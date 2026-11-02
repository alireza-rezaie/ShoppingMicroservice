namespace Authentication.Core.Entities;

public sealed class AuthUser
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
}