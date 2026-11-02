using Authentication.Core.Repositories;
using Authentication.Core.Services;
using MediatR;

namespace Authentication.Application.Commands.Logout;

public sealed class LogoutCommandHandler(
    IAuthenticationRepository repository,
    ITokenService tokenService) : IRequestHandler<LogoutCommand, bool>
{
    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var stored = await repository.GetRefreshTokenAsync(tokenService.HashRefreshToken(request.RefreshToken), cancellationToken);
        if (stored is null) return true;
        if (stored.UserId != request.UserId) throw new UnauthorizedAccessException("The refresh token does not belong to this user.");
        if (stored.RevokedAtUtc is null)
        {
            stored.RevokedAtUtc = DateTime.UtcNow;
            await repository.UpdateRefreshTokenAsync(stored, cancellationToken);
        }
        return true;
    }
}