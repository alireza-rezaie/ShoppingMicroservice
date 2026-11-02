using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.Core.Entities;
using Authentication.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Infrustructure.Services;

public sealed class TokenService(IConfiguration configuration) : ITokenService
{
    private IConfigurationSection Jwt => configuration.GetSection("Jwt");

    public DateTime GetAccessTokenExpiry() =>
        DateTime.UtcNow.AddMinutes(int.TryParse(Jwt["AccessTokenMinutes"], out var minutes) ? minutes : 15);

    public DateTime GetRefreshTokenExpiry() =>
        DateTime.UtcNow.AddDays(int.TryParse(Jwt["RefreshTokenDays"], out var days) ? days : 7);

    public string CreateAccessToken(AuthUser user, IReadOnlyList<string> roles, DateTime expiresAtUtc)
    {
        var key = Jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Email)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var token = new JwtSecurityToken(Jwt["Issuer"], Jwt["Audience"], claims,
            expires: expiresAtUtc, signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public string HashRefreshToken(string refreshToken) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
}