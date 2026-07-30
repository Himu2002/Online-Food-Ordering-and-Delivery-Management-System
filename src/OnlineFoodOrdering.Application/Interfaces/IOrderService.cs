using OnlineFoodOrdering.Application.DTOs.Orders;

namespace OnlineFoodOrdering.Application.Interfaces;

/// <summary>
/// Defines order operations.
/// </summary>
public interface IOrderService
{
    Task<IReadOnlyList<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<OrderDto?> GetByIdAsync(int orderId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);

    Task<OrderDto> CreateAsync(CreateOrderDto request, CancellationToken cancellationToken = default);

    Task<OrderDto?> UpdateAsync(int orderId, UpdateOrderDto request, CancellationToken cancellationToken = default);

    Task<OrderDto?> UpdateStatusAsync(int orderId, UpdateOrderStatusDto request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int orderId, CancellationToken cancellationToken = default);
}