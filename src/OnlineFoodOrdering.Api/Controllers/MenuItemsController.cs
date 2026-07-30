using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodOrdering.Application.DTOs.MenuItems;
using OnlineFoodOrdering.Application.Interfaces;

namespace OnlineFoodOrdering.Api.Controllers;

/// <summary>
/// Manages menu items.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemsController"/> class.
    /// </summary>
    /// <param name="menuItemService">The menu item service.</param>
    public MenuItemsController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    /// <summary>
    /// Gets all menu items.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MenuItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MenuItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _menuItemService.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    /// <summary>
    /// Gets a menu item by identifier.
    /// </summary>
    [HttpGet("{menuItemId:int}")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> GetById(int menuItemId, CancellationToken cancellationToken)
    {
        var item = await _menuItemService.GetByIdAsync(menuItemId, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>
    /// Creates a new menu item.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<MenuItemDto>> Create([FromBody] CreateMenuItemDto request, CancellationToken cancellationToken)
    {
        var item = await _menuItemService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { menuItemId = item.MenuItemId }, item);
    }

    /// <summary>
    /// Updates a menu item.
    /// </summary>
    [HttpPut("{menuItemId:int}")]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> Update(int menuItemId, [FromBody] UpdateMenuItemDto request, CancellationToken cancellationToken)
    {
        var item = await _menuItemService.UpdateAsync(menuItemId, request, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>
    /// Deletes a menu item.
    /// </summary>
    [HttpDelete("{menuItemId:int}")]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int menuItemId, CancellationToken cancellationToken)
    {
        var deleted = await _menuItemService.DeleteAsync(menuItemId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}