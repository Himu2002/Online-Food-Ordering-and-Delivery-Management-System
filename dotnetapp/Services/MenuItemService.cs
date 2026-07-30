using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Api.Data;
using OnlineFoodOrdering.Api.DTOs;
using OnlineFoodOrdering.Api.Models;

namespace OnlineFoodOrdering.Api.Services;

/// <summary>
/// Implements menu item operations.
/// </summary>
public class MenuItemService : IMenuItemService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public MenuItemService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<MenuItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems
            .AsNoTracking()
            .OrderBy(menuItem => menuItem.Name)
            .Select(MapToDto)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<MenuItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var menuItem = await _context.MenuItems
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.MenuItemId == id, cancellationToken);

        return menuItem is null ? null : MapToDto(menuItem);
    }

    /// <inheritdoc />
    public async Task<MenuItemDto> CreateAsync(CreateMenuItemDto dto, CancellationToken cancellationToken = default)
    {
        var menuItem = new MenuItem
        {
            Name = dto.Name.Trim(),
            Category = dto.Category.Trim(),
            Price = dto.Price,
            IsAvailable = dto.IsAvailable
        };

        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(menuItem);
    }

    /// <inheritdoc />
    public async Task<MenuItemDto?> UpdateAsync(int id, UpdateMenuItemDto dto, CancellationToken cancellationToken = default)
    {
        var menuItem = await _context.MenuItems.SingleOrDefaultAsync(item => item.MenuItemId == id, cancellationToken);

        if (menuItem is null)
        {
            return null;
        }

        menuItem.Name = dto.Name.Trim();
        menuItem.Category = dto.Category.Trim();
        menuItem.Price = dto.Price;
        menuItem.IsAvailable = dto.IsAvailable;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(menuItem);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var menuItem = await _context.MenuItems.SingleOrDefaultAsync(item => item.MenuItemId == id, cancellationToken);

        if (menuItem is null)
        {
            return false;
        }

        _context.MenuItems.Remove(menuItem);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

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