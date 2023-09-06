namespace Netflow.Services;

/// <summary>
/// Represents an interface for a background process service that can be executed asynchronously.
/// </summary>
public interface IBackgroundProcessService
{

    /// <summary>
    /// Executes the background process asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RunAsync();
}