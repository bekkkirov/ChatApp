using ChatApp.Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Application.Common.Interfaces;

public interface IUserService
{
    Task<ImageModel> SetProfileImageAsync(string userName, IFormFile image);

    Task<ImageModel> SetBackgroundImageAsync(string userName, string fingerPrint, IFormFile image);

    Task<CurrentUserModel> GetUserInfoAsync(string userName, string fingerPrint);

    Task UpdateUserInfoAsync(string userName, UserUpdateModel updateData);
}