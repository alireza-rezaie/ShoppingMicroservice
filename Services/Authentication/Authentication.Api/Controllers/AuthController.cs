using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.Api.Contracts;
using Authentication.Api.Data;
using Authentication.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    AuthDbContext db,
    IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Email and password are required." });

        var user = new ApplicationUser
        {
            UserName = request.Email.Trim(),
            Email = request.Email.Trim(),
            FullName = request.FullName
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole("Customer"));
            if (!roleResult.Succeeded) return Problem("Could not create the default Customer role.");
        }
        await userManager.AddToRoleAsync(user, "Customer");
        return Ok(await CreateTokens(user));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized(new { message = "Invalid email or password." });
        return Ok(await CreateTokens(user));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var hash = HashToken(request.RefreshToken);
        var stored = await db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash);
        if (stored is null || stored.RevokedAtUtc is not null || stored.ExpiresAtUtc <= DateTime.UtcNow)
            return Unauthorized(new { message = "Refresh token is invalid or expired." });

        stored.RevokedAtUtc = DateTime.UtcNow;
        var user = await userManager.FindByIdAsync(stored.UserId);
        if (user is null) return Unauthorized();
        return Ok(await CreateTokens(user));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(RefreshRequest request)
    {
        var hash = HashToken(request.RefreshToken);
        var stored = await db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash);
        if (stored is null) return NoContent();
        if (stored.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();
        if (stored.RevokedAtUtc is null)
        {
            stored.RevokedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
        return NoContent();
    }

    private async Task<AuthResponse> CreateTokens(ApplicationUser user)
    {
        var jwt = configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var accessMinutes = int.TryParse(jwt["AccessTokenMinutes"], out var minutes) ? minutes : 15;
        var refreshDays = int.TryParse(jwt["RefreshTokenDays"], out var days) ? days : 7;
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Email ?? user.UserName ?? user.Id)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expires = DateTime.UtcNow.AddMinutes(accessMinutes);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(jwt["Issuer"], jwt["Audience"], claims,
            expires: expires, signingCredentials: credentials);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshExpires = DateTime.UtcNow.AddDays(refreshDays);
        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(refreshToken),
            ExpiresAtUtc = refreshExpires
        });
        await db.SaveChangesAsync();
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(jwtToken), expires,
            refreshToken, refreshExpires, user.Email ?? string.Empty, roles);
    }

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}