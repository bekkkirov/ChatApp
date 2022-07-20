namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a channel.
/// </summary>
public class Channel : Chat
{
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