using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineFoodOrdering.Api.Models;

/// <summary>
/// Represents a customer order.
/// </summary>
public class Order
{
    /// <summary>
    /// Gets or sets the unique order identifier.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the date and time the order was placed.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Gets or sets the ordered items.
    /// </summary>
    public List<OrderItem> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the order status.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
