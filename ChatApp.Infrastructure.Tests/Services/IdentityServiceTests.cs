using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChatApp.Application.Common.Exceptions;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models.Authorization;
using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Identity.Entities;
using ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ChatApp.Infrastructure.Tests.Services;

public class IdentityServiceTests
{
    private readonly Mock<UserManager<UserIdentity>> _userManagerMock;
    private readonly Mock<SignInManager<UserIdentity>> _signInManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock = new Mock<ITokenService>();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
    private readonly IMapper _mapper;
    private readonly IdentityService _identityService;

    #region TestData

    private static readonly List<UserIdentity> _userIdentities = new List<UserIdentity>()
    {
        new UserIdentity()
        {
            Id = 1,
            UserName = "user1",
            NormalizedUserName = "USER1",
            Email = "user1@mail.com",
            RefreshToken = new RefreshToken()
            {
                Token = "refreshToken1",
                ExpiryDate = DateTime.UtcNow.AddDays(5)
            }
        },

        new UserIdentity()
        {
            Id = 2,
            UserName = "user2",
            NormalizedUserName = "USER2",
            RefreshToken = new RefreshToken()
            {
                Token = "refreshToken2",
                ExpiryDate = DateTime.UtcNow.AddDays(15)
            }
        },

        new UserIdentity()
        {
            Id = 3,
            UserName = "user3",
            NormalizedUserName = "USER3",
        },
    };

    public static IEnumerable<object[]> SignInAsync_InvalidUserNames_TestData()
    {
        yield return new object[] { new SignInModel() { UserName = "user12414", Password = "pass" } };
        yield return new object[] { new SignInModel() { UserName = "someUser", Password = "jkas" } };
        yield return new object[] { new SignInModel() { UserName = "thisUserDoesn'tExist", Password = "qwer" } };
        yield return new object[] { new SignInModel() { UserName = "abcdef", Password = "pass124" } };
    }

    public static IEnumerable<object[]> SignInAsync_InvalidPasswords_TestData()
    {
        foreach (var identity in _userIdentities)
        {
            yield return new object[] { new SignInModel() { UserName = identity.UserName, Password = "InvalidPassword" } };
        }
    }

    public static IEnumerable<object[]> SignInAsync_ValidAuthorizationData()
    {
        yield return new object[] { new SignInModel() { UserName = "user1", Password = "TotallyValidPassword" } };
        yield return new object[] { new SignInModel() { UserName = "user3", Password = "TotallyValidPassword" } };
    }

    private static SignUpModel _signUpData = new SignUpModel()
    {
        UserName = "newUser",
        Password = "password12345",
        BirthDate = new DateTime(1980, 11, 12),
        Email = "mail@mail.com",
        FirstName = "Jon",
        LastName = "Doe"
    };

    #endregion

    public IdentityServiceTests()
    {
        _mapper = UnitTestHelpers.CreateMapperProfile();

        _userManagerMock = UnitTestHelpers.CreateUserManagerMock();

        _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync((string uName) => _userIdentities.FirstOrDefault(u => u.UserName == uName));

        _userManagerMock.Setup(x => x.Users)
                        .Returns(_userIdentities.BuildMock);

        _signInManagerMock = UnitTestHelpers.CreateSignInManagerMock();


        _tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<string>()))
                         .Returns("AccessToken");

        _tokenServiceMock.Setup(x => x.GenerateRefreshToken())
                         .Returns("RefreshToken");

        _identityService = new IdentityService(_userManagerMock.Object,
                                               _signInManagerMock.Object,
                                               _tokenServiceMock.Object,
                                               _unitOfWorkMock.Object,
                                               _mapper);
    }

    [Theory]
    [MemberData(nameof(SignInAsync_InvalidUserNames_TestData))]
    public async Task SignInAsync_ThrowsWithInvalidUserName(SignInModel signInData)
    {
        await Assert.ThrowsAsync<IdentityException>(() => _identityService.SignInAsync(signInData));
    }

    [Theory]
    [MemberData(nameof(SignInAsync_InvalidPasswords_TestData))]
    public async Task SignInAsync_ThrowsWithInvalidPassword(SignInModel signInData)
    {
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<UserIdentity>(), It.IsAny<string>(), It.IsAny<bool>()))
                          .ReturnsAsync(SignInResult.Failed);

        await Assert.ThrowsAsync<IdentityException>(() => _identityService.SignInAsync(signInData));
    }

    [Theory]
    [MemberData(nameof(SignInAsync_ValidAuthorizationData))]
    public async Task SignInAsync_ShouldWorkWithValidData(SignInModel signInData)
    {
        // Arrange
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<UserIdentity>(), It.IsAny<string>(), It.IsAny<bool>()))
                          .ReturnsAsync(SignInResult.Success);

        // Act
        var tokens = await _identityService.SignInAsync(signInData);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(tokens.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(tokens.RefreshToken));
    }

    [Fact]
    public async Task SignUpAsync_ShouldThrowWhenRegistrationFailed()
    {
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError() {Description = "SomeError"}));

        await Assert.ThrowsAsync<IdentityException>(() => _identityService.SignUpAsync(_signUpData));
    }

    [Fact]
    public async Task SignUpAsync_ShouldWorkWithValidData()
    {
        // Arrange
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        _unitOfWorkMock.Setup(x => x.UserRepository.Add(It.IsAny<User>()));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync());

        // Act
        var tokens = await _identityService.SignUpAsync(_signUpData);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(tokens.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(tokens.RefreshToken));
    }

    [Fact]
    public async Task ChangeEmail_ShouldThrowWhenOperationFailed()
    {
        var userName = "user1";
        var email = "someMail@mail.com";

        _userManagerMock.Setup(x => x.GenerateChangeEmailTokenAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()))
                        .ReturnsAsync("SomeResetToken");

        _userManagerMock.Setup(x => x.ChangeEmailAsync(It.IsAny<UserIdentity>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Failed());

        await Assert.ThrowsAsync<IdentityException>(() => _identityService.ChangeEmailAsync(userName, email));
    }

    [Fact]
    public async Task ChangeEmail_ShouldWorkWithValidData()
    {
        // Arrange
        var userName = "user1";
        var email = "someMail@mail.com";

        _userManagerMock.Setup(x => x.GenerateChangeEmailTokenAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()))
                        .ReturnsAsync("SomeResetToken");

        _userManagerMock.Setup(x => x.ChangeEmailAsync(It.IsAny<UserIdentity>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        // Act
        await _identityService.ChangeEmailAsync(userName, email);

        // Assert
        _userManagerMock.Verify(x => x.ChangeEmailAsync(It.Is<UserIdentity>(u => u.UserName == userName),
                                It.IsAny<string>(),
                                   It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ChangePassword_ShouldThrowWhenThePasswordsAreTheSame()
    {
        var userName = "user1";
        var password = "ThisPasswordIsTheSameAsTheOldOne";

        _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()))
                        .ReturnsAsync(true);

        await Assert.ThrowsAsync<IdentityException>(() => _identityService.ChangePasswordAsync(userName, password));
    }

    [Fact]
    public async Task ChangePassword_ShouldThrowWhenOperationFailed()
    {
        var userName = "user1";
        var password = "ThisPasswordIsTheSameAsTheOldOne";

        _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()))
                        .ReturnsAsync(false);

        _userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Failed());

        await Assert.ThrowsAsync<IdentityException>(() => _identityService.ChangePasswordAsync(userName, password));
    }

    [Fact]
    public async Task ChangePassword_ShouldWorkWithValidData()
    {
        // Arrange
        var userName = "user1";
        var password = "newPassword";

        _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()))
                        .ReturnsAsync(false);

        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<UserIdentity>()))
                        .ReturnsAsync("SomeResetToken");

        _userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        // Act
        await _identityService.ChangePasswordAsync(userName, password);

        // Assert
        _userManagerMock.Verify(x => x.ResetPasswordAsync(It.Is<UserIdentity>(u => u.UserName == userName),
                                    It.IsAny<string>(), 
                               It.IsAny<string>()), Times.Once);
    }
}