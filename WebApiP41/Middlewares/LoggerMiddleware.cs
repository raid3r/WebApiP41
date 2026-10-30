using WebApiP41.Services;
namespace WebApiP41.Middlewares;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestLoggerService _requestLoggerService;
    public LoggerMiddleware(RequestDelegate next, RequestLoggerService requestLoggerService)
    {
        _next = next;
        _requestLoggerService = requestLoggerService;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        await _requestLoggerService.LogRequestAsync();
        await _next(context);
    }
}
