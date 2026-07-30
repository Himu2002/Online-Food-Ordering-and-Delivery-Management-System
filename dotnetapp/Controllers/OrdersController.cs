using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodOrdering.Api.DTOs;
using OnlineFoodOrdering.Api.Services;

namespace OnlineFoodOrdering.Api.Controllers;

/// <summary>
/// Exposes order endpoints.
/// </summary>
[ApiController]
[Authorize]
[Route("api/orders")]
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
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Gets an order by identifier.
    /// </summary>
    /// <param name="id">The order identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(id, cancellationToken);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    /// <summary>
    /// Gets the order history for a customer.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpGet("customer/{customerId:int}")]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderDto>>> GetByCustomerId(int customerId, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetByCustomerIdAsync(customerId, cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="dto">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _orderService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.OrderId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">The order identifier.</param>
    /// <param name="dto">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> Update(int id, [FromBody] UpdateOrderDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _orderService.UpdateAsync(id, dto, cancellationToken);

            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an order.
    /// </summary>
    /// <param name="id">The order identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _orderService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Updates the status of an order.
    /// </summary>
    /// <param name="id">The order identifier.</param>
    /// <param name="dto">The status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _orderService.UpdateStatusAsync(id, dto, cancellationToken);

            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}