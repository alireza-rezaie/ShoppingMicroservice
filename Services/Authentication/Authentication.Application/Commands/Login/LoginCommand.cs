using Authentication.Application.Responses;
using MediatR;

namespace Authentication.Application.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;