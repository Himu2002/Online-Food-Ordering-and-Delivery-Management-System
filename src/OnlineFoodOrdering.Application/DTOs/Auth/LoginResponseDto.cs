namespace OnlineFoodOrdering.Application.DTOs.Auth;

/// <summary>
/// Represents a successful login response.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Gets or sets the JWT access token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token expiration time.
    /// </summary>
    public DateTime ExpiresAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the authenticated user's role.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}