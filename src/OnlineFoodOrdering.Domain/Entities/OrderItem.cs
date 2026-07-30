namespace OnlineFoodOrdering.Domain.Entities;

/// <summary>
/// Represents an item within an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Gets or sets the order item identifier.
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