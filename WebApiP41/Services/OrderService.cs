using Microsoft.EntityFrameworkCore;
using WebApiP41.Models;

namespace WebApiP41.Services;

public class OrderService(ApiDbContext dbContext)
{
    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns>Orders collection</returns>
    public async Task<ICollection<Order>> GetAllOrdersAsync()
    {
        return await dbContext.Orders.Include(o => o.Product).ToListAsync();
    }

    public async Task<Order> AddOrderAsync(Order newOrder)
    {
        newOrder.OrderDateTime = DateTime.Now;
        newOrder.IsCompleted = false; // New order is always not completed
        await dbContext.Orders.AddAsync(newOrder);
        await dbContext.SaveChangesAsync();
        return newOrder;
    }

    public async Task<Order?> UpdateOrderQuantityAsync(int id, int newQuantity)
    {
        var order = await dbContext.Orders.Include(o => o.Product).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return null;
        }
        order.ProductQuantity = newQuantity;
        await dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateOrderStatusAsync(int id, bool isCompleted)
    {
        var order = await dbContext.Orders.Include(o => o.Product).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return null;
        }
        order.IsCompleted = isCompleted;
        await dbContext.SaveChangesAsync();
        return order;
    }
}
