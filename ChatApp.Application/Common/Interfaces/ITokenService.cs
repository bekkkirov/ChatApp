using System.Security.Claims;

namespace ChatApp.Application.Common.Interfaces;

/// <summary>
/// Represents a token service.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates access token for the specified user.
    /// </summary>
    /// <param name="userName">Username.</param>
    /// <returns>Access token.</returns>
    string GenerateAccessToken(string userName);

    /// <summary>
    /// Generates refresh token.
    /// </summary>
    /// <returns>Refresh token.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Generates new access token using refresh token.
    /// </summary>
    /// <param name="token">Expired access token.</param>
    /// <param name="refreshToken">Refresh token.</param>
    /// <returns>New access token.</returns>
    string RefreshToken(string token, string refreshToken);

    /// <summary>
    /// Gets the user principal from the expired access token.
    /// </summary>
    /// <param name="token">Access token.</param>
    /// <returns>User principal.</returns>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}