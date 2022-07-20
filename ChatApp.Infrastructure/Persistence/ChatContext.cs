using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence;

/// <summary>
/// Represents a main database context.
/// </summary>
public class ChatContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<UserSettings> UserSettings { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<Chat> Chats { get; set; }

    public DbSet<Channel> Channels { get; set; }

    public DbSet<Image> Images { get; set; }

    public ChatContext(DbContextOptions<ChatContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatContext).Assembly);
    }
}