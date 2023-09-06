using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflow.Entities;
using Netflow.Infrastructure.Databases;

namespace Netflow.Services;

/// <summary>
/// Represents a service responsible for changing workflow step statuses asynchronously.
/// This class inherits from <see cref="BackgroundProcessService"/> and implements <see cref="IWorkflowStepStatusChangerService"/>.
/// </summary>
public class WorkflowStepStatusChangerService : BackgroundProcessService, IWorkflowStepStatusChangerService
{
    private DataContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowStepStatusChangerService"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DataContext"/> used for data access.</param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/> used for logging.</param>
    public WorkflowStepStatusChangerService(DataContext context, ILogger<WorkflowStepStatusChangerService> logger)
    : base(logger)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the next execution interval for the workflow step status changer service.
    /// </summary>
    public TimeSpan? NextExecutionInterval { [DebuggerStepThrough] get; [DebuggerStepThrough] private set; }

    /// <summary>
    /// Executes the workflow step status changer service asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task ProcessAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            var workflows = await GetWorkflowsAsync(now);

            var stepsToProcess = workflows.Count();
            Logger.LogInformation($"Found {stepsToProcess} step(s) awaiting processing.");

            foreach (var workflow in workflows)
            {
                ProcessWorkflowStepWithTimeThreshold(workflow);
            }

            await _context.SaveChangesAsync();

            Logger.LogInformation($"Fetching the scheduled time for the next execution.");
            var nextTime = await _context.WorkflowsInstances.Where(WorkflowInstance.ToExpireStepAfterPredicate(now))
                                                            .OrderBy(_ => _.CurrentStep.WorkflowStepTimeLimit.ExpiresIn)
                                                            .Select(_ => (DateTime?)_.CurrentStep.WorkflowStepTimeLimit.ExpiresIn)
                                                            .FirstOrDefaultAsync();

            if (nextTime == null)
            {
                Logger.LogInformation($"No scheduled time found.");
                return;
            }

            NextExecutionInterval = nextTime.Value - DateTime.UtcNow;
            Logger.LogInformation($"Next scheduled time for {nextTime}; setting sleep duration to {NextExecutionInterval}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing workflow steps.");
        }
    }

    private void ProcessWorkflowStepWithTimeThreshold(WorkflowInstance workflow)
    {
        Logger.LogInformation("Processing step", workflow);

        if (workflow.CurrentStep.WorkflowStepTimeLimit.AutoApproveOnThreshold)
        {
            Logger.LogInformation($"Automatically approving due to time threshold {workflow.CurrentStep.WorkflowStepTimeLimit.ExpiresIn}", workflow);

            workflow.ApproveStep("", "");
            return;
        }

        Logger.LogInformation($"Automatically rejecting due to time threshold {workflow.CurrentStep.WorkflowStepTimeLimit.ExpiresIn}", workflow);
        workflow.RejectStep("", "");
    }

    private Task<WorkflowInstance[]> GetWorkflowsAsync(DateTime now)
    {
        return _context.WorkflowsInstances.Where(WorkflowInstance.ExpiredStepBeforePredicate(now))
                                        .Include(_ => _.BaseWorkflow)
                                        .Include(_ => _.CurrentStep)
                                        .Include(_ => _.CurrentStep.Approvals)
                                        .Include(_ => _.CurrentStep.BaseStep)
                                        .Include(_ => _.CurrentStep.BaseStep.ApprovedNextStep)
                                        .Include(_ => _.CurrentStep.BaseStep.RejectedNextStep)
                                        .Include(_ => _.CurrentStep.WorkflowStepTimeLimit)
                                        .ToArrayAsync();
    }
}

internal static class CurrentLogMethods
{
    internal static void LogInformation(this ILogger logger, string msg, WorkflowInstance workflow)
    {
        var information = $"{msg} {workflow.CurrentStep} from {workflow}";
        logger.LogInformation(information);
    }

}
