namespace WebApiP41.Models;

public class Order
{
    public int Id { get; set; }
    public int OrderNumber { get; set; } // 01-000123
    public int ProductQuantity { get; set; }
    public decimal ProductPrice { get; set; }
    public DateTime OrderDateTime { get; set; }
    public bool IsCompleted { get; set; }
    public string Comment { get; set; } = string.Empty;
}
