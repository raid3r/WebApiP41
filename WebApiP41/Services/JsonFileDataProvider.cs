using WebApiP41.Models;
namespace WebApiP41.Services;

// Конкретна реалізація IDataProvider для роботи з продуктами, яка зберігає дані у JSON-файлі
public class JsonFileDataProvider<T>: IDataProvider<T>
{
    private readonly string dataFile;

    public JsonFileDataProvider(string dataFile)
    {
        this.dataFile = dataFile;
    }

    public async Task<ICollection<T>> GetAllAsync()
    {
        if (!File.Exists(dataFile))
        {
            return new List<T>();
        }
        var json = await File.ReadAllTextAsync(dataFile);
        return System.Text.Json.JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public async Task SaveChanges(ICollection<T> items)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(items);
        await File.WriteAllTextAsync(dataFile, json);
    }
}
