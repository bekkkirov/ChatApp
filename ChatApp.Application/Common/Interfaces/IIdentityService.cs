using ChatApp.Application.Common.Models.Authorization;

namespace ChatApp.Application.Common.Interfaces;

/// <summary>
/// Represents a user authentication service.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Authorizes the user.
    /// </summary>
    /// <param name="signInData">Auth data.</param>
    /// <returns>Access and refresh tokens for the authorized user.</returns>
    public Task<TokensModel> SignInAsync(SignInModel signInData);

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="signUpData">Register data.</param>
    /// <returns>Access and refresh tokens for the new user.</returns>
    public Task<TokensModel> SignUpAsync(SignUpModel signUpData);
}