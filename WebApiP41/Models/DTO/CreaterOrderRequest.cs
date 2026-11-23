using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiP41.Models.DTO;

/// <summary>
/// DTO model for create order
/// </summary>
public class CreaterOrderRequest
{
    [Description("Product in the order.")]
    public int ProductId { get; set; }

    [Description("The quantity of products in the order.")]
    public int ProductQuantity { get; set; }
    [Description("The price of a single product in the order.")]
    public decimal ProductPrice { get; set; }
    [Description("Additional comments for the order.")]
    public string Comment { get; set; } = string.Empty;
}
