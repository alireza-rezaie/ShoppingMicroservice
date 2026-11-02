using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrustructure.Data;

public sealed class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<ApplicationIdentityUser, IdentityRole, string>(options)
{
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<RefreshTokenEntity>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.TokenHash).IsUnique();
            entity.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        });
    }
}

public sealed class RefreshTokenEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
}