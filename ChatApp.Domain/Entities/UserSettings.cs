using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents the users settings.
/// </summary>
public class UserSettings : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique finger print that allows to identify users device.
    /// </summary>
    public string FingerPrint { get; set; }

    /// <summary>
    /// Gets or sets the background image.
    /// </summary>
    public Image BackgroundImage { get; set; }

    /// <summary>
    /// Gets or sets the user id.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    public User User { get; set; }
}