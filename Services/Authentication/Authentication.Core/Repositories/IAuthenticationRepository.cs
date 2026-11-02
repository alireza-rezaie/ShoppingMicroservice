using Authentication.Core.Entities;

namespace Authentication.Core.Repositories;

public interface IAuthenticationRepository
{
    Task<AuthUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<AuthUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> CreateUserAsync(AuthUser user, string password, CancellationToken cancellationToken = default);
    Task<bool> CheckPasswordAsync(string userId, string password, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetRolesAsync(string userId, CancellationToken cancellationToken = default);
    Task EnsureRoleAsync(string roleName, CancellationToken cancellationToken = default);
    Task AddToRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task AddRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task UpdateRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default);
}