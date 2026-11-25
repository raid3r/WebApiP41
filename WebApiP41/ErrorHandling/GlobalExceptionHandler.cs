using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApiP41.ErrorHandling;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService
    ) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        /*
         * Що тут робимо:
         * 
         * - записати виключення в лог (файл, базу даних, в зовнішню систему моніторингу)
         * - відправити повідомлення адміністратору або команді підтримки (електронна пошта, Slack, Teams)
         * - сформувати відповідь для клієнта (HTTP статус, повідомлення про помилку)
         * 
         */
        logger.LogError(exception, "Unhandled exception occurred while processing the request.");

        if (exception is OrderNotAcceptedByTimeException orderNotAcceptedEx)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Not operation time",
                Detail = orderNotAcceptedEx.Message,
                Type = "https://example.com/probs/invalid-operation",
                Extensions = { { "startHour", orderNotAcceptedEx.StartHour }, { "endHour", orderNotAcceptedEx.EndHour } }
            };
            return await problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails
                }
            );
        }

        //if (exception is ValidationException validationException)
        //{
        //    var problemDetails = new ValidationProblemDetails(validationException.Errors.ToDictionary(
        //        error => error.PropertyName,
        //        error => new[] { error.ErrorMessage }
        //    ))
        //    {
        //        Status = StatusCodes.Status400BadRequest,
        //        Title = "Validation Error",
        //        Type = "https://example.com/probs/validation-error"
        //    };
        //    return await problemDetailsService.TryWriteAsync(
        //        new ProblemDetailsContext
        //        {
        //            HttpContext = httpContext,
        //            ProblemDetails = problemDetails
        //        }
        //    );
        //}

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        if (exception.Message == "Test exception")
        {
            return await problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "An unexpected error occurred.",
                        Detail = exception.Message,
                        Type = "https://example.com/probs/unexpected-error"
                    }
                }
            );
        }
        return false; // Якщо не оброблено, повертаємо false,
                      // щоб дозволити іншим обробникам спробувати обробити виключення
    }
}
