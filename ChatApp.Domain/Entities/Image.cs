namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents an image.
/// </summary>
public class Image
{
    /// <summary>
    /// Gets or sets the url for this image.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the public id for this image.
    /// </summary>
    public string PublicId { get; set; }

    /// <summary>
    /// Gets or sets the user profile for this image.
    /// </summary>
    public UserProfile Profile { get; set; }

    /// <summary>
    /// Gets or sets the message id for this image.
    /// </summary>
    public int? MessageId { get; set; }

    /// <summary>
    /// Gets or sets the message that this image belongs to.
    /// </summary>
    public Message Message { get; set; }

    /// <summary>
    /// Gets or sets the channel id for this image.
    /// </summary>
    public int? ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the channel for this image.
    /// </summary>
    public Channel Channel { get; set; }

    /// <summary>
    /// Gets or sets the user settings id. 
    /// </summary>
    public int? UserSettingsId { get; set; }

    /// <summary>
    /// Gets or sets the user settings.
    /// </summary>
    public UserSettings UserSettings { get; set; }
}