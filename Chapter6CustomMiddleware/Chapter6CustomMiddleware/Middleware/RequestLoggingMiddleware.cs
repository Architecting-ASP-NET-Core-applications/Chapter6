namespace Chapter6CustomMiddleware.Middleware;

/// <summary>
/// Example for page 15
/// </summary>
//public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
//{
//    public async Task InvokeAsync(HttpContext context)
//    {
//        // Log the start of the request
//        logger.LogInformation("------------------------------------------");
//        logger.LogInformation($"Handling request: {context.Request.Path}");

//        // Call the next middleware in the pipeline
//        await next(context);

//        // Log the completion of the request
//        logger.LogInformation("Finished handling request.");
//        logger.LogInformation("------------------------------------------");
//    }
//}



/// <summary>
/// Example for page 16
/// </summary>
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this
                                IApplicationBuilder builder)
    {
        builder.UseMiddleware<FailedRequestWrapperMiddleware>();
        //builder.UseMiddleware<RequestLoggingMiddleware>();
        return builder;
    }
}

/// <summary>
/// Example for page 17
/// </summary>
public class FailedRequestWrapperMiddleware(RequestDelegate next, ILogger<FailedRequestWrapperMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api/v1/"))
        {
            context.Response.StatusCode = 404; // Example status code
            await context.Response.WriteAsync("Custom Not Found Response");
            logger.LogWarning($"404 Not Found: {context.Request.Path}");
        }
        else
        {
            logger.LogInformation($"Request as: {context.Request.Path}");
            await next(context);
        }
    }
}




public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            _logger.LogWarning($"404 Not Found: {context.Request.Path}");

            var errorResponse = new { message = "Risorsa non trovata" };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}