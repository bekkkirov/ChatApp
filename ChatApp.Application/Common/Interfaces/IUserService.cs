using ChatApp.Application.Common.Models;
using ChatApp.Application.Common.Models.Image;
using ChatApp.Application.Common.Models.User;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Application.Common.Interfaces;

/// <summary>
/// Represents a user service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Sets the profile image for the specified user.
    /// </summary>
    /// <returns>New profile image.</returns>
    Task<ImageModel> SetProfileImageAsync(string userName, IFormFile image);

    /// <summary>
    /// Sets the background image for the specified user.
    /// </summary>
    /// <returns>New background image.</returns>
    Task<ImageModel> SetBackgroundImageAsync(string userName, string fingerPrint, IFormFile image);

    /// <summary>
    /// Gets info about specified user.
    /// </summary>
    /// <returns>Info about specified user.</returns>
    Task<CurrentUserModel> GetUserInfoAsync(string userName, string fingerPrint);

    /// <summary>
    /// Updates the specified user.
    /// </summary>
    Task UpdateUserInfoAsync(string userName, UserUpdateModel updateData);
}