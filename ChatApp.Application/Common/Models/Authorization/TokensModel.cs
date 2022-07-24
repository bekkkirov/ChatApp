namespace ChatApp.Application.Common.Models.Authorization;

/// <summary>
/// Represents a model that stores the tokens. 
/// </summary>
public class TokensModel
{
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    public string AccessToken { get; set; }


    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string RefreshToken { get; set; }
}