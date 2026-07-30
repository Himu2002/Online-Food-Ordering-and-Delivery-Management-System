using OnlineFoodOrdering.Application.DTOs.MenuItems;

namespace OnlineFoodOrdering.Application.Interfaces;

/// <summary>
/// Defines menu item operations.
/// </summary>
public interface IMenuItemService
{
    Task<IReadOnlyList<MenuItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<MenuItemDto?> GetByIdAsync(int menuItemId, CancellationToken cancellationToken = default);

    Task<MenuItemDto> CreateAsync(CreateMenuItemDto request, CancellationToken cancellationToken = default);

    Task<MenuItemDto?> UpdateAsync(int menuItemId, UpdateMenuItemDto request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int menuItemId, CancellationToken cancellationToken = default);
}