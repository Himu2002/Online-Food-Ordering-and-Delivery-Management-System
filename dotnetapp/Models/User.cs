namespace OnlineFoodOrdering.Api.Models;

/// <summary>
/// Represents an application user.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique user identifier.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stored password hash or plain-text value for the case study.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user role.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
