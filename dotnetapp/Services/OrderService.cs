using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Api.Data;
using OnlineFoodOrdering.Api.DTOs;
using OnlineFoodOrdering.Api.Models;

namespace OnlineFoodOrdering.Api.Services;

/// <summary>
/// Implements order management operations.
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

    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Include(order => order.Items)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync(cancellationToken);

        return orders.Select(MapToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<OrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Include(existingOrder => existingOrder.Items)
            .SingleOrDefaultAsync(existingOrder => existingOrder.OrderId == id, cancellationToken);

        return order is null ? null : MapToDto(order);
    }

    /// <inheritdoc />
    public async Task<List<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Include(order => order.Items)
            .Where(order => order.CustomerId == customerId)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync(cancellationToken);

        return orders.Select(MapToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<OrderDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Items.Count == 0)
        {
            throw new InvalidOperationException("At least one order item is required.");
        }

        var normalizedItems = await ValidateAndNormalizeItemsAsync(dto.Items.Select(item => (item.MenuItemId, item.Quantity)), cancellationToken);

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var order = new Order
        {
            CustomerId = dto.CustomerId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending"
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        var orderItems = normalizedItems
            .Select(item => new OrderItem
            {
                OrderId = order.OrderId,
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity
            })
            .ToList();

        _context.OrderItems.AddRange(orderItems);
        await _context.SaveChangesAsync(cancellationToken);

        order.Items = orderItems;
        await transaction.CommitAsync(cancellationToken);

        return MapToDto(order);
    }

    /// <inheritdoc />
    public async Task<OrderDto?> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(existingOrder => existingOrder.Items)
            .SingleOrDefaultAsync(existingOrder => existingOrder.OrderId == id, cancellationToken);

        if (order is null)
        {
            return null;
        }

        if (dto.Items.Count == 0)
        {
            throw new InvalidOperationException("At least one order item is required.");
        }

        var normalizedItems = await ValidateAndNormalizeItemsAsync(dto.Items.Select(item => (item.MenuItemId, item.Quantity)), cancellationToken);

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        _context.OrderItems.RemoveRange(order.Items);
        await _context.SaveChangesAsync(cancellationToken);

        var orderItems = normalizedItems
            .Select(item => new OrderItem
            {
                OrderId = order.OrderId,
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity
            })
            .ToList();

        order.CustomerId = dto.CustomerId;
        order.Items = orderItems;

        _context.OrderItems.AddRange(orderItems);
        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return MapToDto(order);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.SingleOrDefaultAsync(existingOrder => existingOrder.OrderId == id, cancellationToken);

        if (order is null)
        {
            return false;
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto, CancellationToken cancellationToken = default)
    {
        if (!AllowedStatuses.Contains(dto.Status))
        {
            throw new InvalidOperationException("Invalid order status.");
        }

        var order = await _context.Orders
            .Include(existingOrder => existingOrder.Items)
            .SingleOrDefaultAsync(existingOrder => existingOrder.OrderId == id, cancellationToken);

        if (order is null)
        {
            return null;
        }

        order.Status = dto.Status;
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(order);
    }

    /// <inheritdoc />
    public async Task<StaffDashboardDto> GetStaffDashboardAsync(CancellationToken cancellationToken = default)
    {
        var pendingOrders = await _context.Orders.CountAsync(order => order.Status == "Pending", cancellationToken);
        var preparingOrders = await _context.Orders.CountAsync(order => order.Status == "Preparing", cancellationToken);

        return new StaffDashboardDto
        {
            PendingOrders = pendingOrders,
            PreparingOrders = preparingOrders
        };
    }

    private async Task<List<(int MenuItemId, int Quantity)>> ValidateAndNormalizeItemsAsync(IEnumerable<(int MenuItemId, int Quantity)> items, CancellationToken cancellationToken)
    {
        var itemList = items.ToList();

        if (itemList.Count == 0)
        {
            throw new InvalidOperationException("At least one order item is required.");
        }

        if (itemList.Any(item => item.Quantity <= 0))
        {
            throw new InvalidOperationException("Order item quantities must be greater than zero.");
        }

        var menuItemIds = itemList.Select(item => item.MenuItemId).Distinct().ToList();
        var menuItems = await _context.MenuItems
            .AsNoTracking()
            .Where(menuItem => menuItemIds.Contains(menuItem.MenuItemId))
            .ToListAsync(cancellationToken);

        if (menuItems.Count != menuItemIds.Count)
        {
            throw new InvalidOperationException("One or more menu items do not exist.");
        }

        var menuItemsById = menuItems.ToDictionary(menuItem => menuItem.MenuItemId);

        foreach (var item in itemList)
        {
            if (!menuItemsById[item.MenuItemId].IsAvailable)
            {
                throw new InvalidOperationException($"Menu item {item.MenuItemId} is not available.");
            }
        }

        return itemList;
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            Status = order.Status,
            Items = order.Items
                .OrderBy(item => item.OrderItemId)
                .Select(item => new OrderItemDto
                {
                    OrderItemId = item.OrderItemId,
                    OrderId = item.OrderId,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity
                })
                .ToList()
        };
    }
}