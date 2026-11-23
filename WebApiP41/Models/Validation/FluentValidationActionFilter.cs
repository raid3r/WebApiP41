using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiP41.Models.Validation;

public class FluentValidationActionFilter : IAsyncActionFilter
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            // Отримуємо тип валідатора для даного аргументу
            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            // Перевіряємо, чи є валідатор у DI контейнері
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
                continue;
            
            // Виконуємо валідацію аргументу
            var result = await validator.ValidateAsync(
                new ValidationContext<object>(argument), context.HttpContext.RequestAborted);

            // Якщо валідація не пройшла, повертаємо помилку 400 Bad Request з деталями валідації
            if (!result.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(result.ToDictionary()));
                return; // Повертаємо помилку 400 Bad Request
            }
        }

        await next();
    }
}
