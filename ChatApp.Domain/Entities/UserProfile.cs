using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a user profile data.
/// </summary>
public class UserProfile : BaseEntity
{
    /// <summary>
    /// Gets or sets the user id for this profile.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the user for this profile.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Gets or sets the "about me" text.
    /// </summary>
    public string About { get; set; }

    /// <summary>
    /// Gets or sets the profile image id.
    /// </summary>
    public int? ProfileImageId { get; set; }

    /// <summary>
    /// Gets or sets the profile image.
    /// </summary>
    public Image ProfileImage { get; set; }

    /// <summary>
    /// Gets or sets the profile background id.
    /// </summary>
    public int? ProfileBackgroundId { get; set; }

    /// <summary>
    /// Gets or sets the profile background.
    /// </summary>
    public Image ProfileBackground { get; set; }
}