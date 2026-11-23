using FluentValidation;
using WebApiP41.Models.DTO;

namespace WebApiP41.Models.Validation;

public class CreaterOrderRequestValidator : AbstractValidator<CreaterOrderRequest>
{
    public CreaterOrderRequestValidator()
    {
        RuleFor(x => x.ProductQuantity)
            .NotNull().WithMessage("Product quantity is required.")
            .GreaterThan(0).WithMessage("Product quantity must be greater than zero.");

        RuleFor(x => x.ProductPrice)
            .NotNull().WithMessage("Product price is required.")
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");

        // Коментар повине бути не пустим рядком,
        // якщо кількість продуктів більше 10 або ціна продукту перевищує 1000

        RuleFor(x => x).CustomAsync(NeedComment);
    }



    private async Task NeedComment(
        CreaterOrderRequest request,
        ValidationContext<CreaterOrderRequest> context,
        CancellationToken cancellationToken
        )
    {
        if (request.ProductQuantity > 10 && string.IsNullOrWhiteSpace(request.Comment.Trim()))
        {
            context.AddFailure(nameof(request.Comment),
                "Comment is required when product quantity is greater than 10.");
        }   

        if (request.ProductPrice > 1000 && string.IsNullOrWhiteSpace(request.Comment.Trim()))
        {
            context.AddFailure(nameof(request.Comment),
                "Comment is required when product price exceeds 1000.");
        }
    }
}



/*
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
 */