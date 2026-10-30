using WebApiP41.Models;
using Microsoft.AspNetCore.Mvc;
using WebApiP41.Services;
using WebApiP41.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Додати сервіси до контейнера залежностей (робота з контроллерами)
builder.Services.AddControllers();

builder.Services.AddSingleton<IDataProvider<Product>>(c => {
    // Простий спосіб отримати значення з конфігурації, якщо ключ відомий (і значення строкове)
    //var dataFile = builder.Configuration["ProductService:DataFilePath"];

    // Більш безпечний спосіб отримати значення з конфігурації, якщо ключ відомий (і значення строкове)
    var dataFile = builder
    .Configuration
    .GetRequiredSection("ProductService")
    .GetValue<string>("DataFilePath");

    // Приклад, якщо в appsettings.json є секція ProductService з параметром Active,
    // який визначає, чи активний сервіс продуктів
    var active = builder
    .Configuration
    .GetRequiredSection("ProductService")
    .GetValue<bool>("Active");

    return new ProductProvider(dataFile);
});

builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<RequestLoggerService>();

// сервер сам себе документує /openapi/v1.json
builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "My API V1");
       
    });
}


/*
 *                          - GET /api/products
 *                          - GET /api/products/{id}
 *    запит - проміжний код - POST /api/products           - відповідь
 *                          - PUT /api/products/{id}
 *                          - DELETE /api/products/{id}
 *                          
 *   проміжний код
 *   
 *   обробник 1            - обробник 2             - обробник 3                           - обробник 4
 *   створює HttpContext    Перевіряє авторизацію     шукає обробник маршруту     виконує обробник маршруту
 *                       ^
 *                      наш обобник (RequestLoggerService) - логування запиту
 *   
 *   Middleware (проміжний код) - це компонент, який обробляє HTTP-запити та відповіді в конвеєрі обробки запитів ASP.NET Core.
 *                          
 */


//app.Use(async(HttpContext context, Func<Task> next) =>
//{
//    // Логування запиту
//    var requestLoggerService = context.RequestServices.GetRequiredService<RequestLoggerService>();
//    await requestLoggerService.LogRequestAsync();
//    // Виклик наступного Middleware у конвеєрі
//    await next.Invoke();
//});
app.UseMiddleware<LoggerMiddleware>();

// Додаємо маршрутизацію для контролерів
app.MapControllers();


app.Run();







/*
 
Microsoft.AspNetCore.OpenApi
Swashbuckle.AspNetCore.SwaggerUI



Архітектурні шаблони побудови застосунків на платформі ASP.NET Core

Web API
(бекенд для фронтенд фреймворків (React, Angular, Vue)), 
бекенд для мобільних застосунків (Xamarin, MAUI), 
бекенд для SPA (Single Page Application) та PWA (Progressive Web Application).

MVC (Model-View-Controller)
великі корпоративні сайти, складний функціонал, різноманітні представлення (Views),
рендеринг html на сервері

Razor Pages
Невеликі сайти, простий функціонал, рендеринг html на сервері

Blazor
Інтерактивні веб-застосунки на C# (WebAssembly),


  


Створити проєкт веб-апі

Для повідомлень

повдомлення
- ід
- від кого (адреса)
- текст повідомлення
- дата час


реалізувати методи отримання усіх повідомлень, 
отримання одного повідомлення по ід, 
повідомленнь від одного відправника (параметр адреса)
видалення повідомленя по ід

стоврити http файл з тестами методів щоб продемонтрувати як вони працюють

  
 */ 