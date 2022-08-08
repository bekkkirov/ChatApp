using AutoMapper;
using ChatApp.Application.Common.Exceptions;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    private readonly UserManager<UserIdentity> _userManager;

    public UserService(IUnitOfWork unitOfWork,
                       IImageService imageService,
                       IMapper mapper,
                       IIdentityService identityService,
                       UserManager<UserIdentity> userManager)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _mapper = mapper;
        _identityService = identityService;
        _userManager = userManager;
    }

    public async Task<ImageModel> SetProfileImageAsync(string userName, IFormFile image)
    {
        var user = await _unitOfWork.UserRepository.GetUserWithProfileImageAsync(userName);
        
        if (user is null)
        {
            throw new ArgumentException("User with specified username doesn't exist.");
        }

        var newImage = await _imageService.AddImageAsync(image);

        if (user.Profile is null)
        {
            user.Profile = new UserProfile();
        }

        if (user.Profile.ProfileImage is not null)
        {
            await _imageService.DeleteImageAsync(user.Profile.ProfileImage.PublicId);
        }

        user.Profile.ProfileImageId = newImage.Id;
        await _unitOfWork.SaveChangesAsync();

        return newImage;
    }

    public async Task<ImageModel> SetBackgroundImageAsync(string userName, string fingerPrint, IFormFile image)
    {
        var user = await _unitOfWork.UserRepository.GetUserWithProfileAndSettingsAsync(userName);
        var settings = user.Settings.FirstOrDefault(s => s.FingerPrint == fingerPrint);

        if (settings is null)
        {
            settings = CreateNewUserSettings(user.Id, fingerPrint);
            user.Settings.Add(settings);
        }

        if (settings.BackgroundImage is not null)
        {
            await _imageService.DeleteImageAsync(settings.BackgroundImage.PublicId);
        }

        var newImage = await _imageService.AddImageAsync(image);
        settings.BackgroundImage = await _unitOfWork.ImageRepository.GetByIdAsync(newImage.Id);

        await _unitOfWork.SaveChangesAsync();

        return newImage;
    }

    public async Task<CurrentUserModel> GetUserInfoAsync(string userName, string fingerPrint)
    {
        var user = await _unitOfWork.UserRepository.GetUserWithProfileAndSettingsAsync(userName);
        var userIdentity = await _userManager.FindByNameAsync(userName);
        var settings = user.Settings.FirstOrDefault(s => s.FingerPrint == fingerPrint);

        if (settings is null)
        {
            settings = CreateNewUserSettings(user.Id, fingerPrint);
            user.Settings.Add(settings);
            await _unitOfWork.SaveChangesAsync();
        }
        
        var userInfo = new CurrentUserModel()
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            About = user.Profile?.About,
            Email = userIdentity.Email,
            ProfileImage = _mapper.Map<ImageModel>(user.Profile?.ProfileImage),
            BackgroundImage = _mapper.Map<ImageModel>(settings.BackgroundImage)
        };

        return userInfo;
    }

    public async Task UpdateUserInfoAsync(string userName, UserUpdateModel updateData)
    {
        var user = await _unitOfWork.UserRepository.GetUserWithProfileImageAsync(userName);

        if (user.Profile is null)
        {
            user.Profile = new UserProfile();
        }

        user.FirstName = updateData.FirstName;
        user.LastName = updateData.LastName;
        user.Profile.About = updateData.About;

        await _unitOfWork.SaveChangesAsync();

        await _identityService.ChangeEmailAsync(userName, updateData.Email);
        await _identityService.ChangePasswordAsync(userName, updateData.Password);
    }

    private UserSettings CreateNewUserSettings(int userId, string fingerPrint)
    {
        var settings = new UserSettings()
        {
            FingerPrint = fingerPrint,
            UserId = userId
        };

        return settings;
    }
}