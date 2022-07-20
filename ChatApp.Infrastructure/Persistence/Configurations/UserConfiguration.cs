using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.UserName)
               .IsUnique();

        builder.Property(u => u.UserName)
               .HasMaxLength(30)
               .IsRequired();

        builder.Property(u => u.FirstName)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(u => u.LastName)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(u => u.BirthDate)
               .IsRequired();

        builder.HasOne(u => u.Profile)
               .WithOne(p => p.User)
               .HasForeignKey<UserProfile>(p => p.UserId);

        builder.HasMany(u => u.Friends)
               .WithMany(u => u.Friends)
               .UsingEntity(x => x.ToTable("Friends"));
    }
}