using Authentication.Application.Responses;
using MediatR;

namespace Authentication.Application.Commands.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;