namespace WebApiP41.Services;

public class RequestLoggerService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestLoggerService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogRequestAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return;
        }

        var logMessage = $"{DateTime.UtcNow}: {httpContext.Request.Method} {httpContext.Request.Path} IP:{httpContext.Connection.RemoteIpAddress}";
        Console.WriteLine(logMessage);
        await File.AppendAllTextAsync("requests.log", logMessage + Environment.NewLine);
    }
}
