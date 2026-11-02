namespace Authentication.Api.Contracts;

public sealed record RegisterRequest(string Email, string Password, string? FullName);
public sealed record LoginRequest(string Email, string Password);
public sealed record RefreshRequest(string RefreshToken);
public sealed record AuthResponse(string AccessToken, DateTime AccessTokenExpiresAtUtc, string RefreshToken, DateTime RefreshTokenExpiresAtUtc, string Email, IList<string> Roles);