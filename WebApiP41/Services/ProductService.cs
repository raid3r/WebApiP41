using WebApiP41.Models;

namespace WebApiP41.Services;

public class ProductService
{
    private readonly string dataFile;

    public ProductService(string dataFile)
    {
        this.dataFile = dataFile;
    }

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

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await LoadProductsAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var products = await LoadProductsAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Product> AddProductAsync(Product newProduct)
    {
        var products = await LoadProductsAsync();
        newProduct.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
        products.Add(newProduct);
        await SaveProductsAsync(products);
        return newProduct;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
    {
        var products = await LoadProductsAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return null;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;
        await SaveProductsAsync(products);
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var products = await LoadProductsAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        products.Remove(product);
        await SaveProductsAsync(products);
        return true;
    }
}
