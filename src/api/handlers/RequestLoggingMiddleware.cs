namespace Netflow.Handlers;

/// <summary>
/// Middleware for logging request information.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger used to log request information.</param>
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Invokes the middleware to log request information and pass the request to the next middleware in the pipeline.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> representing the current request and response.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        // Capture and log request information
        var requestMethod = context.Request.Method;
        var requestPath = context.Request.Path;
        var queryParameters = context.Request.Query;

        _logger.LogDebug("Request Method: {RequestMethod}", requestMethod);
        _logger.LogDebug("Request Path: {RequestPath}", requestPath);
        if (queryParameters.Any())
            _logger.LogDebug("Query Parameters: {QueryParameters}", queryParameters);

        await _next(context);
    }
}