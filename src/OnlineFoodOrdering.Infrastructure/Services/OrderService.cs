using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Application.DTOs.Orders;
using OnlineFoodOrdering.Application.Interfaces;
using OnlineFoodOrdering.Domain.Entities;
using OnlineFoodOrdering.Infrastructure.Persistence;

namespace OnlineFoodOrdering.Infrastructure.Services;

/// <summary>
/// Provides order management operations.
/// </summary>
public class OrderService : IOrderService
{
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Pending",
        "Preparing",
        "Out for Delivery",
        "Delivered"
    };

    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public OrderService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync(cancellationToken);

        return orders.Select(MapToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<OrderDto?> GetByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

        return order is null ? null : MapToDto(order);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync(cancellationToken);

        return orders.Select(MapToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<OrderDto> CreateAsync(CreateOrderDto request, CancellationToken cancellationToken = default)
    {
        ValidateOrderItems(request.Items);

        var menuItemIds = request.Items.Select(x => x.MenuItemId).ToList();
        var existingMenuItemIds = await _dbContext.MenuItems
            .Where(x => menuItemIds.Contains(x.MenuItemId))
            .Select(x => x.MenuItemId)
            .ToListAsync(cancellationToken);

        if (existingMenuItemIds.Count != menuItemIds.Count)
        {
            throw new InvalidOperationException("One or more menu items do not exist.");
        }

        var order = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            Items = request.Items.Select(item => new OrderItem
            {
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity
            }).ToList()
        };

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _dbContext.Entry(order).Collection(x => x.Items).LoadAsync(cancellationToken);
        return MapToDto(order);
    }

    /// <inheritdoc />
    public async Task<OrderDto?> UpdateAsync(int orderId, UpdateOrderDto request, CancellationToken cancellationToken = default)
    {
        ValidateOrderItems(request.Items);

        var order = await _dbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

        if (order is null)
        {
            return null;
        }

        order.CustomerId = request.CustomerId;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            ValidateStatus(request.Status);
            order.Status = request.Status;
        }

        _dbContext.OrderItems.RemoveRange(order.Items);
        order.Items = request.Items.Select(item => new OrderItem
        {
            OrderId = order.OrderId,
            MenuItemId = item.MenuItemId,
            Quantity = item.Quantity
        }).ToList();

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _dbContext.Entry(order).Collection(x => x.Items).LoadAsync(cancellationToken);
        return MapToDto(order);
    }

    /// <inheritdoc />
    public async Task<OrderDto?> UpdateStatusAsync(int orderId, UpdateOrderStatusDto request, CancellationToken cancellationToken = default)
    {
        ValidateStatus(request.Status);

        var order = await _dbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

        if (order is null)
        {
            return null;
        }

        order.Status = request.Status;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(order);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

        if (order is null)
        {
            return false;
        }

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static void ValidateOrderItems(IEnumerable<CreateOrderItemDto> items)
    {
        if (items is null || !items.Any())
        {
            throw new ArgumentException("At least one order item is required.");
        }

        foreach (var item in items)
        {
            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Order item quantity must be greater than zero.");
            }
        }
    }

    private static void ValidateStatus(string status)
    {
        if (!AllowedStatuses.Contains(status))
        {
            throw new ArgumentException("Invalid order status.");
        }
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            Status = order.Status,
            Items = order.Items.Select(item => new OrderItemDto
            {
                OrderItemId = item.OrderItemId,
                OrderId = item.OrderId,
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity
            }).ToList()
        };
    }
}