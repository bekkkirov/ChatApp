using System.Security.Claims;
using ChatApp.Application.Common.Models.Authorization;

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
    /// Generates new access token using the refresh token.
    /// </summary>
    /// <param name="userName">Username.</param>
    /// <param name="tokens">User tokens.</param>
    /// <returns>New access token.</returns>
    Task<string> RefreshToken(string userName, TokensModel tokens);

    /// <summary>
    /// Gets the user principal from the expired access token.
    /// </summary>
    /// <param name="token">Access token.</param>
    /// <returns>User principal.</returns>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}