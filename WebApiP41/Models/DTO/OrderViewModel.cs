namespace WebApiP41.Models.DTO;

public class OrderViewModel
{
    public int Id { get; set; }
    public string OrderNumberFormatted { get; set; }
    public int ProductQuantity { get; set; }
    public decimal ProductPrice { get; set; }
    public decimal TotalPrice {get; set;}
    public DateTime OrderDateTime { get; set; }
    public string CompletedStatus {get; set; }
    public string Comment { get; set; } = string.Empty;

    public ProductView? Product { get; set; }

}
