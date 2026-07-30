namespace OnlineFoodOrdering.Domain.Entities;

/// <summary>
/// Represents a customer order.
/// </summary>
public class Order
{
    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the order creation date.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Gets or sets the ordered line items.
    /// </summary>
    public List<OrderItem> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the order status.
    /// </summary>
    public string Status { get; set; } = "Pending";
}