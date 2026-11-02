using Authentication.Application.Responses;
using Authentication.Core.Repositories;
using Authentication.Core.Services;
using MediatR;

namespace Authentication.Application.Commands.Refresh;

public sealed class RefreshTokenCommandHandler(
    IAuthenticationRepository repository,
    ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hash = tokenService.HashRefreshToken(request.RefreshToken);
        var stored = await repository.GetRefreshTokenAsync(hash, cancellationToken);
        if (stored is null || stored.RevokedAtUtc is not null || stored.ExpiresAtUtc <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token is invalid or expired.");

        var user = await repository.GetByIdAsync(stored.UserId, cancellationToken);
        if (user is null) throw new UnauthorizedAccessException("The account no longer exists.");

        stored.RevokedAtUtc = DateTime.UtcNow;
        await repository.UpdateRefreshTokenAsync(stored, cancellationToken);

        var roles = await repository.GetRolesAsync(user.Id, cancellationToken);
        var accessExpiry = tokenService.GetAccessTokenExpiry();
        var refreshExpiry = tokenService.GetRefreshTokenExpiry();
        var refreshToken = tokenService.CreateRefreshToken();
        await repository.AddRefreshTokenAsync(new Authentication.Core.Entities.RefreshToken
        {
            UserId = user.Id,
            TokenHash = tokenService.HashRefreshToken(refreshToken),
            ExpiresAtUtc = refreshExpiry
        }, cancellationToken);

        return new AuthResponse(tokenService.CreateAccessToken(user, roles, accessExpiry),
            accessExpiry, refreshToken, refreshExpiry, user.Email, roles);
    }
}