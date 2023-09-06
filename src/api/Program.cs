using Netflow.Infrastructure.Logs;

namespace Netflow;

/// <summary>
/// The entry point class for the application.
/// </summary>
public class Program
{
    /// <summary>
    /// Protected constructor of the <see cref="Program"/> class.
    /// </summary>
    protected Program() { }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    public static async Task Main(string[] args)
    {
        // Create the host builder with default configurations.
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                // Specify the startup class for the web host.
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(loggerBuilder =>
            {
                // Configure logging to use a simple console logger with custom settings.
                loggerBuilder.ClearProviders()
                            .AddConsole(options => options.FormatterName = "customName")
                            .AddConsoleFormatter<CustomConsoleFormatter, CustomConsoleFormatterOptions>(o =>
                            {
                                o.TimestampFormat = "[dd/MM/yy HH:mm:ss:fff]";
                            });
            })
            .Build();

        // Run the application.
        await host.RunAsync();
    }
}
