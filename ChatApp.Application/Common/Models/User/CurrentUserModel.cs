using ChatApp.Application.Common.Models.Image;

namespace ChatApp.Application.Common.Models.User;

/// <summary>
/// Represents a data of the current user.
/// </summary>
public class CurrentUserModel
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the fist name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the "about" text.
    /// </summary>
    public string About { get; set; }

    /// <summary>
    /// Gets or sets the profile image.
    /// </summary>
    public ImageModel ProfileImage { get; set; }

    /// <summary>
    /// Gets or sets the background image.
    /// </summary>
    public ImageModel BackgroundImage { get; set; }

}