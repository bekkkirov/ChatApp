using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(p => p.About)
               .HasMaxLength(100);

        builder.HasOne(p => p.ProfileImage)
               .WithOne()
               .HasForeignKey<UserProfile>(p => p.ProfileImageId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.ProfileBackground)
               .WithOne()
               .HasForeignKey<UserProfile>(p => p.ProfileBackgroundId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}