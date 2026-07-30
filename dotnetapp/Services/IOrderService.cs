using OnlineFoodOrdering.Api.DTOs;

namespace OnlineFoodOrdering.Api.Services;

/// <summary>
/// Provides order operations.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Gets all orders.
    /// </summary>
    Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an order by identifier.
    /// </summary>
    Task<OrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets orders for a specific customer.
    /// </summary>
    Task<List<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    Task<OrderDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    Task<OrderDto?> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an order.
    /// </summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an order status.
    /// </summary>
    Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the staff dashboard summary.
    /// </summary>
    Task<StaffDashboardDto> GetStaffDashboardAsync(CancellationToken cancellationToken = default);
}