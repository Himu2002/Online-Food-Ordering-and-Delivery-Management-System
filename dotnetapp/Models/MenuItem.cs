namespace OnlineFoodOrdering.Api.Models;

/// <summary>
/// Represents an item that can be ordered from the menu.
/// </summary>
public class MenuItem
{
    /// <summary>
    /// Gets or sets the unique menu item identifier.
    /// </summary>
    public int MenuItemId { get; set; }

    /// <summary>
    /// Gets or sets the item name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the menu category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is available.
    /// </summary>
    public bool IsAvailable { get; set; }
}
