using System.Security.Claims;
using Authentication.Application.Commands.Login;
using Authentication.Application.Commands.Logout;
using Authentication.Application.Commands.Refresh;
using Authentication.Application.Commands.Register;
using Authentication.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers;

public sealed class AuthController(IMediator mediator) : ApiController
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        try { return Ok(await mediator.Send(command, cancellationToken)); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        try { return Ok(await mediator.Send(command, cancellationToken)); }
        catch (UnauthorizedAccessException ex) { return Unauthorized(new { message = ex.Message }); }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        try { return Ok(await mediator.Send(command, cancellationToken)); }
        catch (UnauthorizedAccessException ex) { return Unauthorized(new { message = ex.Message }); }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        try
        {
            await mediator.Send(new LogoutCommand(userId, request.RefreshToken), cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    public sealed record LogoutRequest(string RefreshToken);
}