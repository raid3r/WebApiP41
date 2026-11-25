using WebApiP41.Services;
namespace WebApiP41.Middlewares;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;   
    private readonly ILogger<LoggerMiddleware> _logger;
    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestLoggerService requestLoggerService)
    {
        _logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
        await requestLoggerService.LogRequestAsync();
        await _next(context);
        _logger.LogInformation("Finished handling request.");
    }
}
