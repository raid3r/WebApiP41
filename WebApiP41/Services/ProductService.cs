using Microsoft.EntityFrameworkCore;
using WebApiP41.Models;

namespace WebApiP41.Services;

public class ProductService(ApiDbContext dbContext)
{
    public async Task<List<Product>> GetAllProductsAsync()
    {
        // використовуємо dbContext для отримання всіх продуктів
        var products = await dbContext.Products.ToListAsync();
        return products;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<Product> AddProductAsync(Product newProduct)
    {
        await dbContext.Products.AddAsync(newProduct);
        await dbContext.SaveChangesAsync();
        return newProduct;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return null;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
