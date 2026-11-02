using Microsoft.AspNetCore.Identity;

namespace Authentication.Api.Models;

public sealed class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}