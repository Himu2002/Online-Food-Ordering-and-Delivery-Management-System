using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodOrdering.Api.DTOs;
using OnlineFoodOrdering.Api.Services;

namespace OnlineFoodOrdering.Api.Controllers;

/// <summary>
/// Exposes staff-only endpoints.
/// </summary>
[ApiController]
[Authorize(Roles = "Staff")]
[Route("api/staff")]
public class StaffController : ControllerBase
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffController"/> class.
    /// </summary>
    /// <param name="orderService">The order service.</param>
    public StaffController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Gets the staff dashboard summary.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(StaffDashboardDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<StaffDashboardDto>> GetDashboard(CancellationToken cancellationToken)
    {
        var dashboard = await _orderService.GetStaffDashboardAsync(cancellationToken);
        return Ok(dashboard);
    }
}