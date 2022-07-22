namespace ChatApp.Infrastructure.Identity.Entities;

/// <summary>
/// Represents a refresh token.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Gets or sets the id for this token.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the creation date for this token.
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the expiry date for this token.
    /// </summary>
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the flag that indicates whether this token was used.
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Gets or sets the user id for this token.
    /// </summary>
    public int UserId { get; set; }
}