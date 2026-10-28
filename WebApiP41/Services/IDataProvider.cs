namespace WebApiP41.Services;

// Узагальнений інтерфейс для провайдера даних (отримання та збереження колекції)
public interface IDataProvider<T>
{
    public Task<ICollection<T>> GetAllAsync();

    public Task SaveChanges(ICollection<T> items);

}
