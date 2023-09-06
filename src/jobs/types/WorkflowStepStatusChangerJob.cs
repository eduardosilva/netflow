using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Netflow.Services;

namespace Netflow.Jobs;

/// <summary>
/// Represents a job responsible for changing workflow step statuses asynchronously.
/// This class implements the <see cref="IWorkflowStepStatusChangerJob"/> interface.
/// </summary>
public class WorkflowStepStatusChangerJob : IWorkflowStepStatusChangerJob
{
    private IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowStepStatusChangerJob"/> class.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve dependencies.</param>
    public WorkflowStepStatusChangerJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the workflow step status changer job asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the job execution.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var logger = _serviceProvider.GetService<ILogger<WorkflowStepStatusChangerJob>>();
            TimeSpan? nextExecutionInterval = null;

            try
            {
                var serviceName = nameof(IWorkflowStepStatusChangerService);
                logger.LogDebug($"Getting {serviceName} instance");

                var service = _serviceProvider.GetService<IWorkflowStepStatusChangerService>();

                logger.LogDebug($"Executing {serviceName} service");
                await service.RunAsync();

                nextExecutionInterval = service.NextExecutionInterval;

                logger.LogDebug($"Completed {serviceName} service");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing this job.");
            }
            finally
            {
                var sleepDuration = nextExecutionInterval.GetValueOrDefault(TimeSpan.FromMinutes(10));
                logger.LogInformation($"Sleeping for {sleepDuration}");
                await Task.Delay(sleepDuration, cancellationToken);
            }
        }
    }
}
