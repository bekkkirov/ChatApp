using ChatApp.Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Identity.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(t => t.Token)
               .HasMaxLength(64)
               .IsRequired();

        builder.HasOne(t => t.User)
               .WithOne(u => u.RefreshToken)
               .HasForeignKey<RefreshToken>(t => t.UserId);
    }
}