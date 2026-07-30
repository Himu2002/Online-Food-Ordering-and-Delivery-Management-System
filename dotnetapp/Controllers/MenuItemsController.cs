using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodOrdering.Api.DTOs;
using OnlineFoodOrdering.Api.Services;

namespace OnlineFoodOrdering.Api.Controllers;

/// <summary>
/// Exposes menu item endpoints.
/// </summary>
[ApiController]
[Authorize]
[Route("api/menuitems")]
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
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<MenuItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MenuItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        var menuItems = await _menuItemService.GetAllAsync(cancellationToken);
        return Ok(menuItems);
    }

    /// <summary>
    /// Gets a menu item by identifier.
    /// </summary>
    /// <param name="id">The menu item identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var menuItem = await _menuItemService.GetByIdAsync(id, cancellationToken);

        if (menuItem is null)
        {
            return NotFound();
        }

        return Ok(menuItem);
    }

    /// <summary>
    /// Creates a new menu item.
    /// </summary>
    /// <param name="dto">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpPost]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<MenuItemDto>> Create([FromBody] CreateMenuItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _menuItemService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.MenuItemId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates a menu item.
    /// </summary>
    /// <param name="id">The menu item identifier.</param>
    /// <param name="dto">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> Update(int id, [FromBody] UpdateMenuItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _menuItemService.UpdateAsync(id, dto, cancellationToken);

            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a menu item.
    /// </summary>
    /// <param name="id">The menu item identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _menuItemService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}