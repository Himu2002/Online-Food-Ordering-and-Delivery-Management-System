namespace OnlineFoodOrdering.Api.DTOs;

/// <summary>
/// Represents the payload used to create a menu item.
/// </summary>
public class CreateMenuItemDto
{
    /// <summary>
    /// Gets or sets the menu item name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the menu item category.
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

/// <summary>
/// Represents the payload used to update a menu item.
/// </summary>
public class UpdateMenuItemDto
{
    /// <summary>
    /// Gets or sets the menu item name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the menu item category.
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

/// <summary>
/// Represents a menu item in API responses.
/// </summary>
public class MenuItemDto
{
    /// <summary>
    /// Gets or sets the menu item identifier.
    /// </summary>
    public int MenuItemId { get; set; }

    /// <summary>
    /// Gets or sets the menu item name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the menu item category.
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