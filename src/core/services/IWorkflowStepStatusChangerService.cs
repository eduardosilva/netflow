namespace Netflow.Services;

/// <summary>
/// Represents an interface for a service responsible for changing workflow step statuses asynchronously.
/// This interface inherits from <see cref="IBackgroundProcessService"/>.
/// </summary>
public interface IWorkflowStepStatusChangerService : IBackgroundProcessService
{
    /// <summary>
    /// Gets the next execution interval for the workflow step status changer service.
    /// </summary>
    public TimeSpan? NextExecutionInterval { get; }
}