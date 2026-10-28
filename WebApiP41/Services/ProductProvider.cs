using WebApiP41.Models;
namespace WebApiP41.Services;


public class ProductProvider: IDataProvider<Product>
{
    private readonly string dataFile;

    public ProductProvider(string dataFile)
    {
        this.dataFile = dataFile;
    }

    public async Task<ICollection<Product>> GetAllAsync()
    {
        if (!File.Exists(dataFile))
        {
            return new List<Product>();
        }
        var json = await File.ReadAllTextAsync(dataFile);
        return System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }

    public async Task SaveChanges(ICollection<Product> items)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(items);
        await File.WriteAllTextAsync(dataFile, json);
    }
}
