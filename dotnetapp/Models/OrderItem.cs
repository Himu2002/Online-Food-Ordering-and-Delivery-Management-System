namespace OnlineFoodOrdering.Api.Models;

/// <summary>
/// Represents a line item within an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Gets or sets the unique order item identifier.
    /// </summary>
    public int OrderItemId { get; set; }

    /// <summary>
    /// Gets or sets the parent order identifier.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the menu item identifier.
    /// </summary>
    public int MenuItemId { get; set; }

    /// <summary>
    /// Gets or sets the ordered quantity.
    /// </summary>
    public int Quantity { get; set; }
}
