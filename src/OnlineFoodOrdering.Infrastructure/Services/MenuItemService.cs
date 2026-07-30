using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Application.DTOs.MenuItems;
using OnlineFoodOrdering.Application.Interfaces;
using OnlineFoodOrdering.Domain.Entities;
using OnlineFoodOrdering.Infrastructure.Persistence;

namespace OnlineFoodOrdering.Infrastructure.Services;

/// <summary>
/// Provides menu item management operations.
/// </summary>
public class MenuItemService : IMenuItemService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public MenuItemService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<MenuItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.MenuItems
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(MapToDtoExpression)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<MenuItemDto?> GetByIdAsync(int menuItemId, CancellationToken cancellationToken = default)
    {
        var menuItem = await _dbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MenuItemId == menuItemId, cancellationToken);

        return menuItem is null ? null : MapToDto(menuItem);
    }

    /// <inheritdoc />
    public async Task<MenuItemDto> CreateAsync(CreateMenuItemDto request, CancellationToken cancellationToken = default)
    {
        var menuItem = new MenuItem
        {
            Name = request.Name,
            Category = request.Category,
            Price = request.Price,
            IsAvailable = request.IsAvailable
        };

        _dbContext.MenuItems.Add(menuItem);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(menuItem);
    }

    /// <inheritdoc />
    public async Task<MenuItemDto?> UpdateAsync(int menuItemId, UpdateMenuItemDto request, CancellationToken cancellationToken = default)
    {
        var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(x => x.MenuItemId == menuItemId, cancellationToken);

        if (menuItem is null)
        {
            return null;
        }

        menuItem.Name = request.Name;
        menuItem.Category = request.Category;
        menuItem.Price = request.Price;
        menuItem.IsAvailable = request.IsAvailable;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(menuItem);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int menuItemId, CancellationToken cancellationToken = default)
    {
        var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(x => x.MenuItemId == menuItemId, cancellationToken);

        if (menuItem is null)
        {
            return false;
        }

        _dbContext.MenuItems.Remove(menuItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static readonly System.Linq.Expressions.Expression<Func<MenuItem, MenuItemDto>> MapToDtoExpression = menuItem => new MenuItemDto
    {
        MenuItemId = menuItem.MenuItemId,
        Name = menuItem.Name,
        Category = menuItem.Category,
        Price = menuItem.Price,
        IsAvailable = menuItem.IsAvailable
    };

    private static MenuItemDto MapToDto(MenuItem menuItem)
    {
        return new MenuItemDto
        {
            MenuItemId = menuItem.MenuItemId,
            Name = menuItem.Name,
            Category = menuItem.Category,
            Price = menuItem.Price,
            IsAvailable = menuItem.IsAvailable
        };
    }
}