using WebApiP41.Models;
namespace WebApiP41.Services;

// Конкретна реалізація IDataProvider для роботи з продуктами, яка зберігає дані у JSON-файлі
public class ProductProvider: JsonFileDataProvider<Product>
{
    public ProductProvider(string dataFile) : base(dataFile)
    {
    }
}
