using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Netflow.Infrastructure.Logs;

/// <summary>
/// Represents a custom console formatter for logging.
/// </summary>
public class CustomConsoleFormatter : ConsoleFormatter, IDisposable
{
    private readonly IDisposable? _optionsReloadToken;
    private readonly string? _assemblyName;
    private CustomConsoleFormatterOptions _formatterOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomConsoleFormatter"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options monitor for the custom console formatter options.</param>
    public CustomConsoleFormatter(IOptionsMonitor<CustomConsoleFormatterOptions> options)
        : base("customName")
    {
        (_optionsReloadToken, _formatterOptions) =
            (options.OnChange(ReloadLoggerOptions), options.CurrentValue);
        _assemblyName = GetType().Assembly.FullName;
    }

    private void ReloadLoggerOptions(CustomConsoleFormatterOptions options) =>
        _formatterOptions = options;

    /// <inheritdoc />
    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        var logLevelString = logEntry.LogLevel.ToString().PadRight(12);
        var timestamp = DateTimeOffset.Now.ToString(_formatterOptions.TimestampFormat);
        var message = logEntry.Formatter(logEntry.State, logEntry.Exception);
        var loggerName = logEntry.Category;

        var threadInfo = $"[Thread-{Thread.CurrentThread.ManagedThreadId}";

        var stackTrace = new StackTrace(true);
        Type? reflectedType = null;
        var count = stackTrace.FrameCount;
        do
        {
            reflectedType = stackTrace?.GetFrame(count - 1)?.GetMethod()?.ReflectedType;
            count--;
            if (count == 0) break;
        }
        while (!(reflectedType?.Assembly.FullName == _assemblyName));

        var className = reflectedType?.FullName;
        var methodName = reflectedType?.Name;
        var locationInfo = !string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(methodName) ?
            $"[{className}.{methodName}]" : string.Empty;

        textWriter.Write($"{timestamp} [{logLevelString}] {threadInfo} [{loggerName}] {locationInfo} - {message}");

        scopeProvider?.ForEachScope<object>(callback: (scope, state) =>
        {
            textWriter.Write(" => ");
            textWriter.Write(scope);
        }, null);

        textWriter.WriteLine();
        if (logEntry.Exception != null)
        {
            textWriter.WriteLine(logEntry.Exception);
        }

    }

    /// <inheritdoc />
    public void Dispose() => _optionsReloadToken?.Dispose();

}
