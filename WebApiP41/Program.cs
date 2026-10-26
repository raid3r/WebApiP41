using WebApiP41.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

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

var products = new List<Product>
{
    new Product { Id = 1, Name = "Product 1", Price = 10.99m, Description = "Description for Product 1" },
    new Product { Id = 2, Name = "Product 2", Price = 19.99m, Description = "Description for Product 2" },
    new Product { Id = 3, Name = "Product 3", Price = 5.99m, Description = "Description for Product 3" }
};


// Отримати всі продукти GET /api/products
app.MapGet("/api/products", () =>
{
    return Results.Ok(products);
});

// Отримати один продукт за ідентифікатором GET /api/products/{id}
// /api/products/1
// /api/products/2
// /api/products/3
app.MapGet("/api/products/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    return Results.Ok(product);
});

// Додати продукт POST /api/products
/*
   {
    "name": "New Product",
    "price": 15.99,
    "description": "Description for New Product"
   }
 */
app.MapPost("/api/products", ([FromBody] Product newProduct) =>
{
    newProduct.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1; // Генеруємо новий Id
    products.Add(newProduct);
    return Results.Ok(newProduct);
});

// Оновити продукт PUT /api/products/{id}
app.MapPut("/api/products/{id:int}", (int id, [FromBody] Product updatedProduct) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    product.Name = updatedProduct.Name;
    product.Price = updatedProduct.Price;
    product.Description = updatedProduct.Description;
    return Results.Ok(product);
});

// Видалити продукт DELETE /api/products/{id}
app.MapDelete("/api/products/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    products.Remove(product);
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