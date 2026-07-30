namespace OnlineFoodOrdering.Api.DTOs;

/// <summary>
/// Represents a single order item in a create request.
/// </summary>
public class CreateOrderItemDto
{
    /// <summary>
    /// Gets or sets the menu item identifier.
    /// </summary>
    public int MenuItemId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }
}

/// <summary>
/// Represents a single order item in an update request.
/// </summary>
public class UpdateOrderItemDto
{
    /// <summary>
    /// Gets or sets the menu item identifier.
    /// </summary>
    public int MenuItemId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }
}

/// <summary>
/// Represents the payload used to create an order.
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

/// <summary>
/// Represents the payload used to update an order.
/// </summary>
public class UpdateOrderDto
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the updated items.
    /// </summary>
    public List<UpdateOrderItemDto> Items { get; set; } = new();
}

/// <summary>
/// Represents a single order item in a response.
/// </summary>
public class OrderItemDto
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
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }
}

/// <summary>
/// Represents an order in API responses.
/// </summary>
public class OrderDto
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
    /// Gets or sets the order date.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Gets or sets the items included in the order.
    /// </summary>
    public List<OrderItemDto> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the current order status.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Represents the payload used to update an order status.
/// </summary>
public class UpdateOrderStatusDto
{
    /// <summary>
    /// Gets or sets the new order status.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Represents the staff dashboard summary.
/// </summary>
public class StaffDashboardDto
{
    /// <summary>
    /// Gets or sets the count of pending orders.
    /// </summary>
    public int PendingOrders { get; set; }

    /// <summary>
    /// Gets or sets the count of preparing orders.
    /// </summary>
    public int PreparingOrders { get; set; }
}