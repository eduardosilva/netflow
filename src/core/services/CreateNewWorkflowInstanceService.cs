using Netflow.Entities;
using Netflow.Infrastructure.Databases;

namespace Netflow.Services;

/// <summary>
/// Service for creating a new instance of a <see cref="Workflow"/>.
/// </summary>
public class CreateNewWorkflowInstanceService : ICreateNewWorkflowInstanceService
{
    private DataContext _dataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateNewWorkflowInstanceService"/> class.
    /// </summary>
    /// <param name="dataContext">The data context used for database operations.</param>
    public CreateNewWorkflowInstanceService(DataContext dataContext)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
    }

    /// <inheritdoc/>
    public async Task<WorkflowInstance?> CreateAsync(Workflow workflow)
    {
        using (var transaction = _dataContext.Database.BeginTransaction())
        {
            try
            {
                if (workflow == null)
                    throw new ArgumentNullException(nameof(workflow));

                await LoadRequiredData(workflow);

                var result = workflow.CreateNewInstance();
                await _dataContext.SaveChangesAsync();
                result.CurrentStep = result.Steps.First();

                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch (ArgumentNullException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new WorkflowCreationException($"Failed to create workflow instance based on {workflow.Id} - {workflow.Name}", ex);
            }
        }
    }

    private async Task LoadRequiredData(Workflow workflow)
    {
        if (!_dataContext.Entry(workflow).Collection(_ => _.Steps).IsLoaded)
        {
            // Load the Steps collection if it's not already loaded
            await _dataContext.Entry(workflow).Collection(_ => _.Steps).LoadAsync();

            // Load the RequiredApprovals collection for each step
            foreach (var step in workflow.Steps)
            {
                if (!_dataContext.Entry(step).Collection(_ => _.RequiredApprovals).IsLoaded)
                {
                    await _dataContext.Entry(step).Collection(_ => _.RequiredApprovals).LoadAsync();
                }
            }
        }
    }
}
