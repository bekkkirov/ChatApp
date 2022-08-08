using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Common.Models.Image;
using ChatApp.Application.Common.Models.User;
using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Identity.Entities;
using ChatApp.Infrastructure.Services;
using ChatApp.Infrastructure.Tests.Comparers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace ChatApp.Infrastructure.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
    private readonly Mock<IImageService> _imageServiceMock = new Mock<IImageService>();
    private readonly IMapper _mapper;
    private readonly Mock<IIdentityService> _identityServiceMock = new Mock<IIdentityService>();
    private readonly Mock<UserManager<UserIdentity>> _userManagerMock;
    private readonly UserService _userService;

    #region TestData

    private static readonly List<User> _users = new List<User>()
    {
        new User()
        {
            UserName = "user1",
            FirstName = "FirstUser1",
            LastName = "LastUser1",
            BirthDate = new DateTime(1999, 12, 1),
            Profile = new UserProfile()
            {
                ProfileImage = new Image()
                {
                    Url = "someUrl",
                    PublicId = "someId"
                }
            },
            Settings = new List<UserSettings>()
        },
        
        new User()
        {
            UserName = "user2",
            FirstName = "FirstUser2",
            LastName = "LastUser2",
            BirthDate = new DateTime(2004, 8, 3),
            Profile = new UserProfile(),
            Settings = new List<UserSettings>()
        },
        
        new User()
        {
            UserName = "user3",
            FirstName = "FirstUser3",
            LastName = "LastUser3",
            BirthDate = new DateTime(1988, 1, 20),
            Profile = new UserProfile(),
            Settings = new List<UserSettings>()
            {
                new UserSettings()
                {
                    FingerPrint = "SomeUniqueFingerPrint",
                    BackgroundImage = new Image()
                    {
                        Url = "SomeUniqueUrl"
                    }
                }
            }
        },
        
        new User()
        {
            UserName = "user4",
            FirstName = "FirstUser4",
            LastName = "LastUser4",
            BirthDate = new DateTime(1977, 4, 15),
            Settings = new List<UserSettings>()
        },
        
        new User()
        {
            UserName = "user5",
            FirstName = "FirstUser5",
            LastName = "LastUser5",
            BirthDate = new DateTime(2000, 10, 10),
            Settings = new List<UserSettings>()
        },
    };

    #endregion

    public UserServiceTests()
    {
        _unitOfWorkMock.Setup(x => x.UserRepository.GetUserWithProfileImageAsync(It.IsAny<string>()))
                       .ReturnsAsync((string uName) => _users.FirstOrDefault(u => u.UserName == uName));

        _unitOfWorkMock.Setup(x => x.UserRepository.GetUserWithProfileAndSettingsAsync(It.IsAny<string>()))
                       .ReturnsAsync((string uName) => _users.FirstOrDefault(u => u.UserName == uName));

        _userManagerMock = UnitTestHelpers.CreateUserManagerMock();
        _mapper = UnitTestHelpers.CreateMapperProfile();

        _userService = new UserService(_unitOfWorkMock.Object, _imageServiceMock.Object, _mapper, _identityServiceMock.Object, _userManagerMock.Object);
    }

    [Theory]
    [InlineData("user999")]
    [InlineData("someUser")]
    [InlineData("user678")]
    [InlineData("someUser123321")]
    public async Task SetProfileImage_ThrowsWhenUserNotFound(string userName)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.SetProfileImageAsync(userName, It.IsAny<IFormFile>()));
    }

    [Theory]
    [InlineData("user1")]
    [InlineData("user3")]
    [InlineData("user4")]
    public async Task SetProfileImage_ShouldWorkWithValidData(string userName)
    {
        // Arrange
        var image = new ImageModel()
        {
            Id = 1,
            Url = "SomeUrl"
        };

        _imageServiceMock.Setup(x => x.AddImageAsync(It.IsAny<IFormFile>()))
                         .ReturnsAsync(image);

        // Act
        var actual = await _userService.SetProfileImageAsync(userName, It.IsAny<IFormFile>());

        // Assert
        Assert.Equal(image.Id, actual.Id);
    }

    [Theory]
    [InlineData("user1", "SomeUniqueFingerPrint")]
    [InlineData("user3", "SomeUniqueFingerPrint")]
    public async Task SetBackgroundImage_ShouldWorkWithValidData(string userName, string fingerPrint)
    {
        // Arrange
        var image = new ImageModel()
        {
            Id = 1,
            Url = "SomeUrl"
        };

        _imageServiceMock.Setup(x => x.AddImageAsync(It.IsAny<IFormFile>()))
                         .ReturnsAsync(image);

        _unitOfWorkMock.Setup(x => x.ImageRepository.GetByIdAsync(It.IsAny<int>()))
                       .ReturnsAsync((int id) => new Image() {Id = id, Url = "SomeUrl"});

        // Act
        var actual = await _userService.SetBackgroundImageAsync(userName, fingerPrint, It.IsAny<IFormFile>());

        // Assert
        Assert.Equal(image.Id, actual.Id);
    }

    [Theory]
    [InlineData("user1", "SomeUniqueFingerPrint")]
    [InlineData("user2", "SomeUniqueFingerPrint")]
    [InlineData("user3", "SomeUniqueFingerPrint")]
    public async Task GetUserInfo_ShouldReturnCorrectData(string userName, string fingerPrint)
    {
        // Arrange
        var email = "mail@mail.com";

        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync(new UserIdentity() {Email = email});

        var user = _users.First(u => u.UserName == userName);

        var expected = new CurrentUserModel()
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            About = user.Profile.About,
            ProfileImage = _mapper.Map<ImageModel>(user.Profile.ProfileImage),
            BackgroundImage = _mapper.Map<ImageModel>(user.Settings.FirstOrDefault(s => s.FingerPrint == fingerPrint)?.BackgroundImage),
            Email = email
        };

        // Act
        var actual = await _userService.GetUserInfoAsync(userName, fingerPrint);

        // Assert
        Assert.Equal(expected, actual, new CurrentUserModelComparer());
    }
}