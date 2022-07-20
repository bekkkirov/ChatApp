using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a channel.
/// </summary>
public class Channel : BaseEntity
{
    /// <summary>
    /// Gets or sets the list of users for this channel.
    /// </summary>
    public List<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// Gets or sets the list of messages for this channel.
    /// </summary>
    public List<Message> Messages { get; set; } = new List<Message>();

    /// <summary>
    /// Gets or sets the id of the creator for this channel.
    /// </summary>
    public int CreatorId { get; set; }

    /// <summary>
    /// Gets or sets the creator for this channel.
    /// </summary>
    public User Creator { get; set; }

    /// <summary>
    /// Gets or sets the icon for this channel.
    /// </summary>
    public Image Icon { get; set; }
}