using ChatApp.Application.Common.Models.Authorization;

namespace ChatApp.Application.Common.Interfaces;

/// <summary>
/// Represents a user authentication service.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Authenticates the user.
    /// </summary>
    /// <param name="signInData">Auth data.</param>
    /// <returns>Access and refresh tokens for the user with provided credentials.</returns>
    public Task<TokensModel> SignInAsync(SignInModel signInData);

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="signUpData">Register data.</param>
    /// <returns>Access and refresh tokens for the new user.</returns>
    public Task<TokensModel> SignUpAsync(SignUpModel signUpData);

    /// <summary>
    /// Changes email for the specified user.
    /// </summary>
    Task ChangeEmailAsync(string userName, string newEmail);

    /// <summary>
    /// Changes password for the specified user.
    /// </summary>
    Task ChangePasswordAsync(string userName, string newPassword);
}