using MediatR;

namespace Authentication.Application.Commands.Logout;

public sealed record LogoutCommand(string UserId, string RefreshToken) : IRequest<bool>;