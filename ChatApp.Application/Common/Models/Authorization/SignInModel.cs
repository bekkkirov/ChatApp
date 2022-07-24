namespace ChatApp.Application.Common.Models.Authorization;

/// <summary>
/// Represents a data used for authorization.
/// </summary>
public class SignInModel
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; }
}