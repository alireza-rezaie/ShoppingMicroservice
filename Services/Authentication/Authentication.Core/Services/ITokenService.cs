using Authentication.Core.Entities;

namespace Authentication.Core.Services;

public interface ITokenService
{
    string CreateAccessToken(AuthUser user, IReadOnlyList<string> roles, DateTime expiresAtUtc);
    string CreateRefreshToken();
    string HashRefreshToken(string refreshToken);
    DateTime GetAccessTokenExpiry();
    DateTime GetRefreshTokenExpiry();
}