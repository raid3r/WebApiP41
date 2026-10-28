using WebApiP41.Models;

namespace WebApiP41.Services;

public class ProductService
{
    private readonly IDataProvider<Product> dataProvider;

    public ProductService(IDataProvider<Product> dataProvider)
    {
        this.dataProvider = dataProvider;
    }
        
    public async Task<List<Product>> GetAllProductsAsync()
    {
        var products = await dataProvider.GetAllAsync();
        return products.ToList();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var products = await dataProvider.GetAllAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Product> AddProductAsync(Product newProduct)
    {
        var products = await dataProvider.GetAllAsync();
        newProduct.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
        products.Add(newProduct);
        await dataProvider.SaveChanges(products);
        return newProduct;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
    {
        var products = await dataProvider.GetAllAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return null;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;
        await dataProvider.SaveChanges(products);
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var products = await dataProvider.GetAllAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        products.Remove(product);
        await dataProvider.SaveChanges(products);
        return true;
    }
}
