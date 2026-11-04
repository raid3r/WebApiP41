using WebApiP41.Services;

namespace WebApiP41.Middlewares;

public class CheckUserMiddleware
{
    private readonly RequestDelegate _next;
    public CheckUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserService userService)
    {
        var headerName = "Authorization";
        var token = context.Request.Headers[headerName].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrWhiteSpace(token))
        {
            await _next(context);
            return;
        }

        var authUser = await userService.ValidateTokenAsync(token);

        if (authUser == null)
        {
            await _next(context);
            return;
        }

        context.Items["User"] = authUser;
        await _next(context);
    }

    /*
     
    запит - HttpContext - міддлвара 1 - міддлвара 2 - контроллер - метод контролера - 
      
     */ 

}
