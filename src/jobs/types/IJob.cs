namespace Netflow.Jobs;

/// <summary>
/// Represents a job interface that can be executed asynchronously.
/// </summary>
public interface IJob
{
    /// <summary>
    /// Executes the job asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the job execution.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RunAsync(CancellationToken cancellationToken);
}