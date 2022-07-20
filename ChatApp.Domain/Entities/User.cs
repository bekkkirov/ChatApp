using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Gets or sets the username for this user.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the first name for this user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name for this user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the birth date for this user.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the profile data for this user.
    /// </summary>
    public UserProfile Profile { get; set; }

    /// <summary>
    /// Gets or sets the list of settings for this user.
    /// </summary>
    public List<UserSettings> Settings { get; set; } = new List<UserSettings>();

    /// <summary>
    /// Gets or sets the list of friends for this user.
    /// </summary>
    public List<User> Friends { get; set; } = new List<User>();

    /// <summary>
    /// Gets or sets the list of chats for this user.
    /// </summary>
    public List<Chat> Chats { get; set; } = new List<Chat>();

    /// <summary>
    /// Gets or sets the list of channels that this user created.
    /// </summary>
    public List<Channel> CreatedChannels { get; set; } = new List<Channel>();

    /// <summary>
    /// Gets or sets the list of channels that this user joined.
    /// </summary>
    public List<Channel> JoinedChannels { get; set; } = new List<Channel>();
}