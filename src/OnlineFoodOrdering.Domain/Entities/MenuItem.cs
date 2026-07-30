namespace OnlineFoodOrdering.Domain.Entities;

/// <summary>
/// Represents a menu item available for ordering.
/// </summary>
public class MenuItem
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
    /// Gets or sets the food category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is currently available.
    /// </summary>
    public bool IsAvailable { get; set; }
}