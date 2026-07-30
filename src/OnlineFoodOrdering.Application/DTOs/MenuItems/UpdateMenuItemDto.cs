namespace OnlineFoodOrdering.Application.DTOs.MenuItems;

/// <summary>
/// Represents the payload for updating a menu item.
/// </summary>
public class UpdateMenuItemDto
{
    /// <summary>
    /// Gets or sets the menu item name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is available.
    /// </summary>
    public bool IsAvailable { get; set; }
}