using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Netflow.Services;

/// <summary>
/// Represents an abstract base class for background process services.
/// This class implements <see cref="IBackgroundProcessService"/>.
/// </summary>
public abstract class BackgroundProcessService : IBackgroundProcessService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundProcessService"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public BackgroundProcessService(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Executes the background process asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await ProcessAsync();
        }
        finally
        {
            stopwatch.Stop();
            Logger.LogInformation("Service processing completed in {ElapsedTime} milliseconds.", stopwatch.ElapsedMilliseconds);
        }
    }
    /// <summary>
    /// Executes the specific background process asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task ProcessAsync();

    /// <summary>
    /// Gets the logger instance used for logging.
    /// </summary>
    public ILogger Logger { [DebuggerStepThrough] get; }
}
