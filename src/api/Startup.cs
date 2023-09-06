using Microsoft.EntityFrameworkCore;
using Netflow.Infrastructure.Databases;
using System.Reflection;
using Netflow.Handlers;
using Swashbuckle.AspNetCore.SwaggerUI;
using Netflow.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netflow;

/// <summary>
/// Represents the startup class for the application.
/// </summary>
public class Startup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Gets the application configuration.
    /// </summary>
    /// <remarks>
    /// The application configuration contains key-value pairs of settings
    /// used throughout the application. It is typically populated from various
    /// sources such as appsettings.json, environment variables, command-line arguments, etc.
    /// </remarks>
    public IConfiguration Configuration { get; }

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
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();
        services.AddAuthorization();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddAutoMapper(typeof(Startup));

        ConfigureControllers(services);
        ConfigureHttpServer(services);
        ConfigureAppServices(services);
        ConfigureSwagger(services);
        ConfigureDatabaseContext(services);
    }

    /// <summary>
    /// Configures JSON serialization options for controllers in the ASP.NET Core application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    private static void ConfigureControllers(IServiceCollection services)
    {
        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Use camelCase for property names
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                    // Ignore null values when writing JSON
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
    }

    /// <summary>
    /// Configures the HTTP server settings for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    private void ConfigureHttpServer(IServiceCollection services)
    {
        // Configure Kestrel server options
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 1024 * 1024; // 1 MB max request body size
            options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2); // 2 minutes keep-alive timeout
            options.Limits.MaxConcurrentConnections = 100; // Limit concurrent connections
            options.Limits.MaxConcurrentUpgradedConnections = 10; // Limit concurrent WebSocket connections
            options.AddServerHeader = false; // Hide the "Server" header
        });
    }

    /// <summary>
    /// Configures application services to be used for dependency injection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    private void ConfigureAppServices(IServiceCollection services)
    {
        services.AddTransient<ICreateNewWorkflowInstanceService, CreateNewWorkflowInstanceService>();
    }

    /// <summary>
    /// Configures the application pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The web host environment.</param>
    /// <remarks>
    /// This method is used to configure the application request pipeline. It is called during the application startup,
    /// and here you can specify how the application should handle incoming HTTP requests and responses.
    /// </remarks>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Use developer exception page if running in development environment.
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(_ =>
            {
                _.SwaggerEndpoint("/swagger/v1/swagger.json", "Netflow API v1");
                _.DocExpansion(DocExpansion.List);
                _.EnableDeepLinking();

                _.OAuthClientId("netflow");
                _.OAuthAppName("Swagger");
            });
        }

        // Use routing middleware.
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHealthChecks("/health");

        // Use endpoint middleware to handle incoming requests.
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    /// <summary>
    /// Configures the database context for the ASP.NET Core application.
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

    /// <summary>
    /// Configures Swagger documentation for the ASP.NET Core application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });
    }
}