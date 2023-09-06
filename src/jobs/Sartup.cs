using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Netflow.Infrastructure.Databases;
using Netflow.Infrastructure.Logs;
using Netflow.Services;

namespace Netflow.Jobs;

/// <summary>
/// Represents the startup class for the application.
/// </summary>
public class Startup
{
    /// <summary>
    /// Configures the application services.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <remarks>
    /// This method is used to configure the application services. It is called during the application startup,
    /// and here you can register various services, dependencies, and options with the dependency injection container.
    /// </remarks>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IWorkflowStepStatusChangerJob, WorkflowStepStatusChangerJob>();
        services.AddTransient<IWorkflowStepStatusChangerService, WorkflowStepStatusChangerService>();

        services.AddLogging(loggerBuilder =>
        {
            // Configure logging to use a simple console logger with custom settings.
            loggerBuilder.AddConsole(options => options.FormatterName = "customName")
                        .AddConsoleFormatter<CustomConsoleFormatter, CustomConsoleFormatterOptions>(o =>
                        {
                            o.TimestampFormat = "[dd/MM/yy HH:mm:ss:fff]";
                        });
        });

        ConfigureDatabaseContext(services);
    }

    /// <summary>
    /// Configures the database context for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    private void ConfigureDatabaseContext(IServiceCollection services)
    {
        // Configure the application's services, including the database context.
        var connectionstring = Environment.GetEnvironmentVariable("CONNECTION_STRING")?.Replace("\"", "");
        if (string.IsNullOrEmpty(connectionstring))
            throw new InvalidOperationException("CONNECTION_STRING environment variable not found");

        // Register the DataContext with Npgsql as the PostgreSQL provider.
        services.AddDbContext<DataContext>(
            options => options.UseNpgsql(connectionstring)
                              .UseSnakeCaseNamingConvention()
                              .EnableSensitiveDataLogging());
    }

}