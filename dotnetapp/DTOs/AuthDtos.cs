namespace OnlineFoodOrdering.Api.DTOs;

/// <summary>
/// Represents the credentials used to sign in.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Represents the result of a successful login.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Gets or sets the JWT bearer token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user role.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}