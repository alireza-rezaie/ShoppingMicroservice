using Authentication.Application.Responses;
using Authentication.Core.Entities;
using Authentication.Core.Repositories;
using Authentication.Core.Services;
using MediatR;

namespace Authentication.Application.Commands.Login;

public sealed class LoginCommandHandler(
    IAuthenticationRepository repository,
    ITokenService tokenService) : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email.Trim(), cancellationToken);
        if (user is null || !await repository.CheckPasswordAsync(user.Id, request.Password, cancellationToken))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var roles = await repository.GetRolesAsync(user.Id, cancellationToken);
        var accessExpiry = tokenService.GetAccessTokenExpiry();
        var refreshExpiry = tokenService.GetRefreshTokenExpiry();
        var refreshToken = tokenService.CreateRefreshToken();
        await repository.AddRefreshTokenAsync(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = tokenService.HashRefreshToken(refreshToken),
            ExpiresAtUtc = refreshExpiry
        }, cancellationToken);

        return new AuthResponse(tokenService.CreateAccessToken(user, roles, accessExpiry),
            accessExpiry, refreshToken, refreshExpiry, user.Email, roles);
    }
}