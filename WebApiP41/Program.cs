using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApiP41.Middlewares;
using WebApiP41.Models;
using WebApiP41.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Додати контекст бази даних до контейнера залежностей
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(connectionString) // Використовуємо SQLite як базу даних
);

// AutoMapper configuration
builder.Services.AddAutoMapper(c => {
   

}, typeof(Program));

// Додати сервіси до контейнера залежностей (робота з контролерами)
builder.Services.AddControllers();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<RequestLoggerService>();


// Додати сервіси до контейнера залежностей
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();

// сервер сам собі документує /openapi/v1.json
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
app.UseMiddleware<CheckUserMiddleware>();
app.UseMiddleware<LoggerMiddleware>();

// Додаємо маршрутизацію для контролерів
app.MapControllers();


app.Run();


/*
 Переробити проєкт про повідомлення з попереднього завдання на використання контролера

---

Створити middleware яке буде рахувати кількість запитів на кожен з ваших роутів
і записувати у файл в форматі json
[
	{
	"method": "GET",
	"route": "/api/messages",
	"count": 1
	},
    {
	"method": "POST",
	"route": "/api/messages",
	"count": 1
	},   
]
Завантажуючі дані з файлу, і зберігаючі їх кожен раз

 */




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

  

клієнт1 - запит - сервер - відповідь - клієнт
клієнт1 - запит - сервер - відповідь - клієнт
клієнт2 - запит - сервер - відповідь - клієнт
клієнт1- запит - сервер - відповідь - клієнт
клієнт3 - запит - сервер - відповідь - клієнт
клієнт3 - запит - сервер - відповідь - клієнт
клієнт1 - запит - сервер - відповідь - клієнт

токен у заголовку запиту (зазвичай Authorization: XXXXXXXXXXXXXXXXXX) 

клієнт формує запит (без заголовку) з логіном та паролем (логін та пароль у тілі запиту)
сервер перевіряє логін та пароль, якщо все ок - сервер формує токен (набір символів)
    + двохфакторна автентифікація (2FA) - додатковий код, який користувач отримує на телефон або пошту
 - унікальний для кожного користувача
 - сервер його пам'ятає (який токен - який користувач кому належить)
токен відправляється клієнту у відповіді (зазвичай у тілі відповіді)
далі клієнт при кожному запиті до сервера відправляє токен у заголовку запиту (зазвичай Authorization: XXXXXXXXXXXXXXXXXX)


Доповнити документацію проєкта

Додати коментарями та атрибутами
- контролери
- методи контролерів
- параметри методів контролера
- DTO моделі
- свойства DTO моделей


---

Додати AutoMapper пакет до проєкту
додати профілі перетворень моделей в DTO і навпаки
використати цей функціонал в своєму проєкті



Створіть модель замовлення з полями:
номер замовлення;
кількість товарів;
вартість товарів;
дата-час замовлення;
виконано (так/ні);
коментар.

Номер замовлення, дату-час замовлення і позначку «виконано» заповнює сервер. 
Нове замовлення завжди невиконане.

Створіть контролер замовлень з методами:
список замовлень;
додати замовлення;
оновити кількість товарів у замовленні;
змінити статус замовлення (виконано/не виконано).

CreaterOrderRequest ->  Order
Order -> OrderViewModel

OperationResult (success, error)

Для кожного методу використовуйте DTO, модель замовлення напряму не приймається і не повертається:
для додавання: лише ті поля, які заповнює клієнт;
для оновлення кількості: лише кількість товарів;
для зміни статусу: лише позначка «виконано»;
для відповіді: поля замовлення плюс текстовий статус «виконано» або «не виконано».
Перетворення між моделлю і DTO описуйте в окремому профілі AutoMapper для замовлень. Поля, які заповнює сервер, у профілі свідомо ігноруються.
Перевірки даних: кількість товарів більша за нуль, вартість більша за нуль, коментар не довший за 200 символів.


Напишіть order.http з методами перевірки роботи контролера замовлень


 */