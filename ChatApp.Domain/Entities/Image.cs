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
    /// Gets or sets the chat id for this image.
    /// </summary>
    public int? ChatId { get; set; }

    /// <summary>
    /// Gets or sets the chat for this image.
    /// </summary>
    public Chat Chat { get; set; }

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