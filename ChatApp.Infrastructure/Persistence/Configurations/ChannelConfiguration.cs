using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.HasOne(c => c.Icon)
               .WithOne(i => i.Channel)
               .HasForeignKey<Image>(i => i.ChannelId);

        builder.HasOne(c => c.Creator)
               .WithMany(u => u.CreatedChannels)
               .HasForeignKey(c => c.CreatorId);

        builder.HasMany(c => c.Users)
               .WithMany(u => u.JoinedChannels)
               .UsingEntity<Dictionary<string, object>>(
                   j => j.HasOne<User>().WithMany().OnDelete(DeleteBehavior.Restrict),
                   j => j.HasOne<Channel>().WithMany().OnDelete(DeleteBehavior.Restrict));
    }
}