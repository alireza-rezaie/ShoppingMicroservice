namespace Authentication.Api.Models;

public sealed class RefreshToken
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required string TokenHash { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
}