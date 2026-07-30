namespace OnlineFoodOrdering.Application.DTOs.Orders;

/// <summary>
/// Represents the payload for updating an order.
/// </summary>
public class UpdateOrderDto
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the updated ordered items.
    /// </summary>
    public List<CreateOrderItemDto> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the order status.
    /// </summary>
    public string Status { get; set; } = "Pending";
}