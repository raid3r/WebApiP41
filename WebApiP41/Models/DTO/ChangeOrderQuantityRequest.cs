using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiP41.Models.DTO;

[Description("Request to change the quantity of a product in an order.")]
public class ChangeOrderQuantityRequest
{
    [Description("The new quantity of the product in the order. Must be greater than zero.")]
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int NewQuantity { get; set; }
}


[Description("Request to change the status of an order.")]
public class ChangeOrderStatusRequest
{
    [Description("The new status of the order. True for completed, false for not completed.")]
    [Required]
    public bool NewStatus { get; set; }
}
