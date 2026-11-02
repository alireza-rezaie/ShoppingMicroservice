using Authentication.Core.Entities;
using Authentication.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrustructure.Data;

public sealed class AuthenticationRepository(
    AuthDbContext db,
    UserManager<ApplicationIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager) : IAuthenticationRepository
{
    public async Task<AuthUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user is null ? null : Map(user);
    }

    public async Task<AuthUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);
        return user is null ? null : Map(user);
    }

    public async Task<IReadOnlyList<string>> CreateUserAsync(AuthUser user, string password, CancellationToken cancellationToken = default)
    {
        var entity = new ApplicationIdentityUser
        {
            UserName = user.Email,
            Email = user.Email,
            FullName = user.FullName
        };
        var result = await userManager.CreateAsync(entity, password);
        if (!result.Succeeded) return result.Errors.Select(x => x.Description).ToArray();
        user.Id = entity.Id;
        return Array.Empty<string>();
    }

    public async Task<bool> CheckPasswordAsync(string userId, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is not null && await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is null ? Array.Empty<string>() : (await userManager.GetRolesAsync(user)).ToArray();
    }

    public async Task EnsureRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
        }
    }

    public async Task AddToRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException("User not found.");
        var result = await userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        var token = await db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
        return token is null ? null : Map(token);
    }

    public async Task AddRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        db.RefreshTokens.Add(new RefreshTokenEntity
        {
            UserId = token.UserId, TokenHash = token.TokenHash,
            ExpiresAtUtc = token.ExpiresAtUtc, RevokedAtUtc = token.RevokedAtUtc
        });
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        var entity = await db.RefreshTokens.FirstOrDefaultAsync(x => x.Id == token.Id, cancellationToken);
        if (entity is null) throw new InvalidOperationException("Refresh token not found.");
        entity.RevokedAtUtc = token.RevokedAtUtc;
        await db.SaveChangesAsync(cancellationToken);
    }

    private static AuthUser Map(ApplicationIdentityUser user) =>
        new() { Id = user.Id, Email = user.Email ?? string.Empty, FullName = user.FullName };

    private static RefreshToken Map(RefreshTokenEntity token) =>
        new() { Id = token.Id, UserId = token.UserId, TokenHash = token.TokenHash,
            ExpiresAtUtc = token.ExpiresAtUtc, RevokedAtUtc = token.RevokedAtUtc };
}