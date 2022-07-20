using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.Property(s => s.FingerPrint)
               .HasMaxLength(50)
               .IsRequired();

        builder.HasOne(s => s.User)
               .WithMany(u => u.Settings)
               .HasForeignKey(s => s.UserId);

        builder.HasOne(u => u.BackgroundImage)
               .WithOne(i => i.UserSettings)
               .HasForeignKey<Image>(i => i.UserSettingsId);
    }
}