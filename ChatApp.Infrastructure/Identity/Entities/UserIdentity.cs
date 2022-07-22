using Microsoft.AspNetCore.Identity;

namespace ChatApp.Infrastructure.Identity.Entities;

/// <summary>
/// Represents a user identity in the identity database.
/// </summary>
public class UserIdentity : IdentityUser<int>
{
    /// <summary>
    /// Gets or sets the refresh token for this user.
    /// </summary>
    public RefreshToken RefreshToken { get; set; }
}