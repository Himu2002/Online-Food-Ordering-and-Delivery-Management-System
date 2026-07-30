using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineFoodOrdering.Api.Data;
using OnlineFoodOrdering.Api.DTOs;
using OnlineFoodOrdering.Api.Models;

namespace OnlineFoodOrdering.Api.Services;

/// <summary>
/// Handles authentication and token generation.
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="configuration">The configuration root.</param>
    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var user = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Username == request.Username && user.PasswordHash == request.Password, cancellationToken);

        if (user is null)
        {
            return null;
        }

        return new LoginResponseDto
        {
            Token = GenerateToken(user),
            Username = user.Username,
            Role = user.Role
        };
    }

    private string GenerateToken(User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
        var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
        var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing.");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
            new("UserId", user.UserId.ToString()),
            new("Username", user.Username),
            new("Role", user.Role)
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}