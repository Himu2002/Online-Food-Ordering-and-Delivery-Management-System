namespace OnlineFoodOrdering.Application.DTOs.Orders;

/// <summary>
/// Represents a single item requested when creating an order.
/// </summary>
public class CreateOrderItemDto
{
    /// <summary>
    /// Gets or sets the menu item identifier.
    /// </summary>
    public int MenuItemId { get; set; }

    /// <summary>
    /// Gets or sets the requested quantity.
    /// </summary>
    public int Quantity { get; set; }
}