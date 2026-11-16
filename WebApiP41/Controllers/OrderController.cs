using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiP41.Services;
using WebApiP41.Models;
using WebApiP41.Models.DTO;
using System.ComponentModel;

namespace WebApiP41.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Order management")]
public class OrderController(OrderService orderService, IMapper mapper) : ControllerBase
{
    /*
     * Створіть контролер замовлень з методами:
+список замовлень;
+додатиp замовлення;
оновити кількість товарів у замовленні;
змінити статус замовлення (виконано/не виконано).
     * 
     */

    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns>Orders</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<OrderViewModel>), StatusCodes.Status200OK)]
    public async Task<ICollection<OrderViewModel>> GetAllOrders()
    {
        var orders = await orderService.GetAllOrdersAsync();
        return orders.Select(order => mapper.Map<OrderViewModel>(order)).ToList();
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOrder([FromBody] CreaterOrderRequest request)
    {
        var newOrder = mapper.Map<Order>(request);
        var addedOrder = await orderService.AddOrderAsync(newOrder);
        return Ok(mapper.Map<OrderViewModel>(addedOrder));
    }

    [HttpPut("{id:int}/quantity")]
    [ProducesResponseType(typeof(OrderViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderQuantity(int id, [FromBody] ChangeOrderQuantityRequest request)
    {
        var updatedOrder = await orderService.UpdateOrderQuantityAsync(id, request.NewQuantity);
        if (updatedOrder == null)
        {
            return NotFound(new OperationResult { Success = false, ErrorMessage = "Order not found." });
        }
        return Ok(mapper.Map<OrderViewModel>(updatedOrder));
    }

    [HttpPut("{id:int}/status")]
    [ProducesResponseType(typeof(OrderViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] ChangeOrderStatusRequest request)
    {
        var updatedOrder = await orderService.UpdateOrderStatusAsync(id, request.NewStatus);
        if (updatedOrder == null)
        {
            return NotFound(new OperationResult { Success = false, ErrorMessage = "Order not found." });
        }
        return Ok(mapper.Map<OrderViewModel>(updatedOrder));
    }
}
