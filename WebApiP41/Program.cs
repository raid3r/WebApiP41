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

var dataFile = "products.json";

async Task<List<Product>> LoadProductsAsync()
{
    if (!File.Exists(dataFile))
    {
        return new List<Product>();
    }
    var json = await File.ReadAllTextAsync(dataFile);
    return System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
}

async Task SaveProductsAsync(List<Product> products)
{
    var json = System.Text.Json.JsonSerializer.Serialize(products);
    await File.WriteAllTextAsync(dataFile, json);
}


// Отримати всі продукти GET /api/products
app.MapGet("/api/products", async () =>
{
    var products = await LoadProductsAsync();
    return Results.Ok(products);
});

// Отримати один продукт за ідентифікатором GET /api/products/{id}
// /api/products/1
// /api/products/2
// /api/products/3
app.MapGet("/api/products/{id:int}", async (int id) =>
{
    var products = await LoadProductsAsync();
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    return Results.Ok(product);
});

// Додати продукт POST /api/products
app.MapPost("/api/products", async ([FromBody] Product newProduct) =>
{
    var products = await LoadProductsAsync();
    newProduct.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1; // Генеруємо новий Id
    products.Add(newProduct);
    await SaveProductsAsync(products);
    return Results.Ok(newProduct);
});

// Оновити продукт PUT /api/products/{id}
app.MapPut("/api/products/{id:int}", async (int id, [FromBody] Product updatedProduct) =>
{
    var products = await LoadProductsAsync();
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    product.Name = updatedProduct.Name;
    product.Price = updatedProduct.Price;
    product.Description = updatedProduct.Description;
    await SaveProductsAsync(products);
    return Results.Ok(product);
});

// Видалити продукт DELETE /api/products/{id}
app.MapDelete("/api/products/{id:int}", async (int id) =>
{
    var products = await LoadProductsAsync();
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    products.Remove(product);
    await SaveProductsAsync(products);
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