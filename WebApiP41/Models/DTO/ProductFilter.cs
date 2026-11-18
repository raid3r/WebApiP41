using System.ComponentModel.DataAnnotations;

namespace WebApiP41.Models.DTO;

public class ProductFilter
{
    public string? Name { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "MinPrice must be greater than or equal to 0")]
    public decimal? MinPrice { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "MaxPrice must be greater than or equal to 0")]
    public decimal? MaxPrice { get; set; }
    public string? Description { get; set; }
}
