using AutoMapper;
using WebApiP41.Models.DTO;

namespace WebApiP41.Models;

public class ProductResolver(ApiDbContext dbContext): IValueResolver<CreaterOrderRequest, Order, Product>
{

    public Product Resolve(CreaterOrderRequest source, Order destination, Product destMember, ResolutionContext context)
    {
        var product = dbContext.Products.FirstOrDefault(p => p.Id == source.ProductId);
        if (product == null)
        {
            throw new ArgumentException($"Product with Id {source.ProductId} not found.");
        }
        return product;
    }

}
