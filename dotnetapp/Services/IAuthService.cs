using OnlineFoodOrdering.Api.DTOs;

namespace OnlineFoodOrdering.Api.Services;

/// <summary>
/// Provides authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user and returns a JWT response when valid.
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The login response, or null when the credentials are invalid.</returns>
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}