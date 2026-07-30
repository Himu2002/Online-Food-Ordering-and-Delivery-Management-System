namespace OnlineFoodOrdering.Application.DTOs.Orders;

/// <summary>
/// Represents the payload for creating an order.
/// </summary>
public class CreateOrderDto
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the ordered items.
    /// </summary>
    public List<CreateOrderItemDto> Items { get; set; } = new();
}