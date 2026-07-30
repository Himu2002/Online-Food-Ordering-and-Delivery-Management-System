using OnlineFoodOrdering.Application.DTOs.Auth;

namespace OnlineFoodOrdering.Application.Interfaces;

/// <summary>
/// Defines authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user and returns a JWT payload when credentials are valid.
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The login response, or <c>null</c> when authentication fails.</returns>
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}