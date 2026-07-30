namespace OnlineFoodOrdering.Application.DTOs.Auth;

/// <summary>
/// Represents a registration request.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the plain-text password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}