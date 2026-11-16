namespace WebApiP41.Models.DTO;

public class PageResult<T> where T : class
{
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T> Items { get; set; }
}
