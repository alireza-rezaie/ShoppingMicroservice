using Microsoft.AspNetCore.Identity;

namespace Authentication.Infrustructure.Data;

public sealed class ApplicationIdentityUser : IdentityUser
{
    public string? FullName { get; set; }
}