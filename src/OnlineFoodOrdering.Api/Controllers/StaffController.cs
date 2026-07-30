using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineFoodOrdering.Api.Controllers;

/// <summary>
/// Represents staff-only operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Staff")]
public class StaffController : ControllerBase
{
    /// <summary>
    /// Returns a simple staff dashboard payload.
    /// </summary>
    /// <returns>A staff dashboard response.</returns>
    [HttpGet("dashboard")]
    public IActionResult GetDashboard()
    {
        return Ok(new
        {
            Message = "Welcome to the staff dashboard."
        });
    }
}