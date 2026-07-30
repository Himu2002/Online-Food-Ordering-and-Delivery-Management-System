using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodOrdering.Application.DTOs.Orders;
using OnlineFoodOrdering.Application.Interfaces;

namespace OnlineFoodOrdering.Api.Controllers;

/// <summary>
/// Manages customer orders.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="orderService">The order service.</param>
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Gets all orders.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(typeof(IReadOnlyList<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Gets a specific order.
    /// </summary>
    [HttpGet("{orderId:int}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetById(int orderId, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(orderId, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    /// <summary>
    /// Gets order history for a customer.
    /// </summary>
    [HttpGet("customer/{customerId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetByCustomerId(int customerId, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetByCustomerIdAsync(customerId, cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Places a food order.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto request, CancellationToken cancellationToken)
    {
        var order = await _orderService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { orderId = order.OrderId }, order);
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    [HttpPut("{orderId:int}")]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> Update(int orderId, [FromBody] UpdateOrderDto request, CancellationToken cancellationToken)
    {
        var order = await _orderService.UpdateAsync(orderId, request, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    /// <summary>
    /// Updates the order status in real time.
    /// </summary>
    [HttpPatch("{orderId:int}/status")]
    [Authorize(Roles = "Staff,DeliveryAgent")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> UpdateStatus(int orderId, [FromBody] UpdateOrderStatusDto request, CancellationToken cancellationToken)
    {
        var order = await _orderService.UpdateStatusAsync(orderId, request, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    /// <summary>
    /// Deletes an order.
    /// </summary>
    [HttpDelete("{orderId:int}")]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int orderId, CancellationToken cancellationToken)
    {
        var deleted = await _orderService.DeleteAsync(orderId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}