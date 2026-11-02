using Authentication.Application.Responses;
using Authentication.Core.Entities;
using Authentication.Core.Repositories;
using Authentication.Core.Services;
using MediatR;

namespace Authentication.Application.Commands.Register;

public sealed class RegisterCommandHandler(
    IAuthenticationRepository repository,
    ITokenService tokenService) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Email and password are required.");

        var email = request.Email.Trim();
        if (await repository.GetByEmailAsync(email, cancellationToken) is not null)
            throw new InvalidOperationException("An account with this email already exists.");

        var user = new AuthUser { Email = email, FullName = request.FullName };
        var errors = await repository.CreateUserAsync(user, request.Password, cancellationToken);
        if (errors.Count > 0) throw new ArgumentException(string.Join("; ", errors));

        await repository.EnsureRoleAsync("Customer", cancellationToken);
        await repository.AddToRoleAsync(user.Id, "Customer", cancellationToken);
        return await IssueTokens(user, new[] { "Customer" }, cancellationToken);
    }

    private async Task<AuthResponse> IssueTokens(AuthUser user, IReadOnlyList<string> roles, CancellationToken cancellationToken)
    {
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