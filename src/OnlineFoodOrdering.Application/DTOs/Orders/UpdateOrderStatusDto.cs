namespace OnlineFoodOrdering.Application.DTOs.Orders;

/// <summary>
/// Represents the payload for updating an order status.
/// </summary>
public class UpdateOrderStatusDto
{
    /// <summary>
    /// Gets or sets the new order status.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}