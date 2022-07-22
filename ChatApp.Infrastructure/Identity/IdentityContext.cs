using ChatApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Identity;

/// <summary>
/// Represents a context for the identity database.
/// </summary>
public class IdentityContext : IdentityDbContext<UserIdentity, UserRole, int>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);
    }
}