using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.Property(m => m.Text)
               .IsRequired();

        builder.Property(m => m.TimeStamp)
               .IsRequired();

        builder.HasOne(m => m.Image)
               .WithOne(i => i.Message)
               .HasForeignKey<Image>(i => i.MessageId);

        builder.HasOne(m => m.Sender)
               .WithMany()
               .HasForeignKey(m => m.SenderId);

        builder.HasOne(m => m.Chat)
               .WithMany(c => c.Messages)
               .HasForeignKey(m => m.ChatId);

        builder.HasOne(m => m.Channel)
               .WithMany(c => c.Messages)
               .HasForeignKey(m => m.ChannelId);
    }
}