using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiP41.Models.DTO;

/// <summary>
/// DTO model for create order
/// </summary>
public class CreaterOrderRequest
{
    [Required(ErrorMessage = "Product quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Product quantity must be greater than zero.")]
    [Description("The quantity of products in the order.")]
    public int ProductQuantity { get; set; }

    [Required(ErrorMessage = "Product price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than zero.")]
    [Description("The price of a single product in the order.")]
    
    public decimal ProductPrice { get; set; }
    [Description("Additional comments for the order.")]
    [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
    public string Comment { get; set; } = string.Empty;
}
