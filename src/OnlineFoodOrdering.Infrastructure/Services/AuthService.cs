using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Application.DTOs.Auth;
using OnlineFoodOrdering.Application.Interfaces;
using OnlineFoodOrdering.Domain.Entities;
using OnlineFoodOrdering.Infrastructure.Persistence;

namespace OnlineFoodOrdering.Infrastructure.Services;

/// <summary>
/// Handles authentication and JWT issuance.
/// </summary>
public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly JwtTokenService _jwtTokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="passwordHasher">The password hasher.</param>
    /// <param name="jwtTokenService">The JWT token generator.</param>
    public AuthService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher, JwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    /// <inheritdoc />
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var (token, expiresAtUtc) = _jwtTokenService.CreateToken(user.UserId, user.Username, user.Role);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAtUtc = expiresAtUtc,
            Role = user.Role
        };
    }
}