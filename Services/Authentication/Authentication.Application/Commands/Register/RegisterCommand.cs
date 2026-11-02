using Authentication.Application.Responses;
using MediatR;

namespace Authentication.Application.Commands.Register;

public sealed record RegisterCommand(string Email, string Password, string? FullName) : IRequest<AuthResponse>;