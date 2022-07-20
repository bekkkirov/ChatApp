using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a chat.
/// </summary>
public class Chat : BaseEntity
{
    /// <summary>
    /// Gets or sets the list of users for this chat.
    /// </summary>
    public List<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// Gets or sets the list of messages for this chat.
    /// </summary>
    public List<Message> Messages { get; set; } = new List<Message>();
}