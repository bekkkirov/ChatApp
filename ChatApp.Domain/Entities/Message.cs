using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a message.
/// </summary>
public class Message : BaseEntity
{
    /// <summary>
    /// Gets or sets the text for this image.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the time stamp for this image.
    /// </summary>
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the sender id for this message.
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// Gets or sets the sender for this message.
    /// </summary>
    public User Sender { get; set; }

    /// <summary>
    /// Gets or sets the image for this message.
    /// </summary>
    public Image Image { get; set; }

    /// <summary>
    /// Gets or sets the chat id for this message.
    /// </summary>
    public int? ChatId { get; set; }

    /// <summary>
    /// Gets or sets the chat for this message.
    /// </summary>
    public Chat Chat { get; set; }

    /// <summary>
    /// Gets or sets the channel id for this message.
    /// </summary>
    public int? ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the channel for this message.
    /// </summary>
    public Channel Channel { get; set; }
}

