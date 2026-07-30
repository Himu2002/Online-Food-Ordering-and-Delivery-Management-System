using OnlineFoodOrdering.Api.DTOs;

namespace OnlineFoodOrdering.Api.Services;

/// <summary>
/// Provides menu item operations.
/// </summary>
public interface IMenuItemService
{
    /// <summary>
    /// Gets all menu items.
    /// </summary>
    Task<List<MenuItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a menu item by identifier.
    /// </summary>
    Task<MenuItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new menu item.
    /// </summary>
    Task<MenuItemDto> CreateAsync(CreateMenuItemDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing menu item.
    /// </summary>
    Task<MenuItemDto?> UpdateAsync(int id, UpdateMenuItemDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a menu item.
    /// </summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}