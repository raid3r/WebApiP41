using WebApiP41.Models;
using Microsoft.AspNetCore.Mvc;
using WebApiP41.Services;

var builder = WebApplication.CreateBuilder(args);

var dataFile = "products.json";
builder.Services.AddSingleton(new ProductService(dataFile));

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






// Отримати всі продукти GET /api/products
app.MapGet("/api/products", async (ProductService productService) =>
{
    var products = await productService.GetAllProductsAsync();
    return Results.Ok(products);
});

// Отримати один продукт за ідентифікатором GET /api/products/{id}
// /api/products/1
// /api/products/2
// /api/products/3
app.MapGet("/api/products/{id:int}", async (int id, ProductService productService) =>
{
    var product = await productService.GetProductByIdAsync(id);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    return Results.Ok(product); // 200 OK
});

// Додати продукт POST /api/products
app.MapPost("/api/products", async ([FromBody] Product newProduct, ProductService productService) =>
{
    var product = await productService.AddProductAsync(newProduct);
    return Results.Ok(product);
});

// Оновити продукт PUT /api/products/{id}
app.MapPut("/api/products/{id:int}", async (int id, [FromBody] Product updatedProduct, ProductService productService) =>
{
    var product = await productService.UpdateProductAsync(id, updatedProduct);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
     return Results.Ok(product);
});

// Видалити продукт DELETE /api/products/{id}
app.MapDelete("/api/products/{id:int}", async (int id, ProductService productService) =>
{
    var result = await productService.DeleteProductAsync(id);
    if (!result)
    {
        return Results.NotFound(); // 404 Not Found
    }
    return Results.Ok();
});

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


  
  
 */ 