using Microsoft.AspNetCore.Mvc;
using WebApiP41.Models;
using WebApiP41.Services;

namespace WebApiP41.Controllers;

[Tags("Product Management")]
[ApiController]
[Route("api/products")] // api/products
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    public bool CheckUser()
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return false;
        }
        return true;
    }


    // Отримати всі продукти GET /api/products
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }

        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }
    
    // Додати продукт POST /api/products
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product newProduct)
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }

        var product = await _productService.AddProductAsync(newProduct);
        return Ok(product);
    }

    // Отримати один продукт за ідентифікатором GET /api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(); // 404 Not Found
        }
        return Ok(product); // 200 OK
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }

        var product = await _productService.UpdateProductAsync(id, updatedProduct);
        if (product == null)
        {
            return NotFound(); // 404 Not Found
        }
        return Ok(product); // 200 OK
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (!CheckUser())
        {
            return Unauthorized();
        }

        var result = await _productService.DeleteProductAsync(id);
        if (!result)
        {
            return NotFound(); // 404 Not Found
        }
        return Ok(); // 200 OK
    }
}
