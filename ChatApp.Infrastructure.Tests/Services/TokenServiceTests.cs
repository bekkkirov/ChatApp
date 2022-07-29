using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApp.Application.Common.Exceptions;
using ChatApp.Application.Common.Models.Authorization;
using ChatApp.Application.Common.Settings;
using ChatApp.Infrastructure.Identity;
using ChatApp.Infrastructure.Identity.Entities;
using ChatApp.Infrastructure.Identity.Extensions;
using ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MockQueryable.Moq;
using Moq;
using Xunit;
using UserIdentity = ChatApp.Infrastructure.Identity.Entities.UserIdentity;

namespace ChatApp.Infrastructure.Tests.Services;

public class TokenServiceTests
{
    private readonly string _key = "VeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVerySecretKey";
    private readonly Mock<UserManager<UserIdentity>> _userManagerMock;
    private readonly TokenValidationParameters _tokenValidationParams;
    private readonly IOptions<TokenSettings> _tokenSettings;

    private readonly TokenService _tokenService;

    #region TestData

    private static readonly List<UserIdentity> _userIdentities = new List<UserIdentity>()
    {
        new UserIdentity()
        {
            Id = 1,
            UserName = "user1",
            NormalizedUserName = "USER1",
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
            RefreshToken = new RefreshToken()
            {
                Token = "refreshToken3",
                ExpiryDate = DateTime.UtcNow.AddDays(25),
            }
        },
    };

    private static readonly List<UserIdentity> _userIdentitiesWithInvalidRefreshTokens = new List<UserIdentity>()
    {
        new UserIdentity()
        {
            Id = 1,
            UserName = "user1",
            NormalizedUserName = "USER1",
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
                ExpiryDate = DateTime.UtcNow.AddDays(-5)
            }
        },
        
        new UserIdentity()
        {
            Id = 3,
            UserName = "user3",
            NormalizedUserName = "USER3",
            RefreshToken = new RefreshToken()
            {
                Token = "refreshToken3",
                ExpiryDate = DateTime.UtcNow.AddDays(15),
                IsUsed = true
            }
        },
        
        new UserIdentity()
        {
            Id = 4,
            UserName = "user4",
            NormalizedUserName = "USER4",
        },
    };

    public static IEnumerable<object[]> RefreshTokenThrows_TestData()
    {
        yield return new object[] { "This user doesn't exist", new TokensModel() { AccessToken = "AccessToken", RefreshToken = "SomeToken" } };

        yield return new object[] { _userIdentitiesWithInvalidRefreshTokens[0].UserName, new TokensModel() { AccessToken = "AccessToken", RefreshToken = _userIdentitiesWithInvalidRefreshTokens[1].RefreshToken?.Token } };

        foreach (var identity in _userIdentitiesWithInvalidRefreshTokens.Skip(1))
        {
            yield return new object[] { identity.UserName, new TokensModel() { AccessToken = "AccessToken", RefreshToken = identity.RefreshToken?.Token } };
        }
    }

    public static IEnumerable<object[]> RefreshToken_TestData()
    {
        var accessTokens = new List<string>()
        {
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJ1c2VyMSIsIm5iZiI6MTY1OTA5MDcyOSwiZXhwIjoxNjYwMzAwMzI5LCJpYXQiOjE2NTkwOTA3Mjl9.3U_bDBjBb_srGCM3sAPVunO_8-uaUT_vnFnQbAVjuXNk2iTxlWSSZf4W1XvOsDt5jyARcnGMmd-oQcGSye_n6g",
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJ1c2VyMiIsIm5iZiI6MTY1OTA5MDY0MCwiZXhwIjoxNjYwMzAwMjQwLCJpYXQiOjE2NTkwOTA2NDB9.m8YhFnautHWjtada76l1FYS3cYbfPB5UATFMaMWKkF0s1CKtxGSWylz00XZM0S8HyRXovEXMTB-SNjmT3jAztA",
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJ1c2VyMyIsIm5iZiI6MTY1OTA5MDc2MSwiZXhwIjoxNjYwMzAwMzYxLCJpYXQiOjE2NTkwOTA3NjF9._r8X0f7iwoNfGLpxBaZGmCZVTJ7iHeAiecsd2J9YpYvrLrlPlcww4gZ9HZ8AjM_BGtSWU92kIetJIyH56dZBhg"
        };

        for (int i = 0; i < _userIdentities.Count; i++)
        {
            yield return new object[]
            {
                _userIdentities[i].UserName,

                new TokensModel()
                {
                    AccessToken = accessTokens[i],
                    RefreshToken = _userIdentities[i].RefreshToken?.Token
                }
            };
        }
    }

    #endregion

    public TokenServiceTests()
    {
        _userManagerMock = UnitTestHelpers.CreateUserManagerMock();
        _tokenSettings = Options.Create(new TokenSettings() { Key = _key });
        _tokenValidationParams = UnitTestHelpers.CreateTokenValidationParameters(_key);

        _tokenService = new TokenService(_tokenSettings, _tokenValidationParams, _userManagerMock.Object);
    }

    [Theory]
    [InlineData("testUser")]
    [InlineData("someRandomUserName")]
    [InlineData("totallyNormalUserName")]
    public void GenerateAccessToken_ShouldReturnValidToken(string userName)
    {
        // Arrange
        var tokenHandler = new JwtSecurityTokenHandler();

        // Act
        var accessToken = _tokenService.GenerateAccessToken(userName);

        // Assert
        var principal = tokenHandler.ValidateToken(accessToken, _tokenValidationParams, out var validatedToken);
        var jwtSecurityToken = validatedToken as JwtSecurityToken;

        Assert.NotNull(jwtSecurityToken);
        Assert.Equal(userName, principal.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnValidToken()
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        Assert.True(!string.IsNullOrWhiteSpace(refreshToken));
    }

    [Theory]
    [MemberData(nameof(RefreshTokenThrows_TestData))]
    public async Task RefreshToken_ShouldThrowValidationExceptionWithInvalidData(string userName, TokensModel tokens)
    {
        var queryable = _userIdentitiesWithInvalidRefreshTokens.BuildMock();

        _userManagerMock.Setup(x => x.Users)
                        .Returns(queryable);

        await Assert.ThrowsAsync<ValidationException>(() => _tokenService.RefreshToken(userName, tokens));
    }

    [Theory]
    [MemberData(nameof(RefreshToken_TestData))]
    public async Task RefreshToken_ShouldReturnNewAccessToken(string userName, TokensModel tokens)
    {
        // Arrange
        var queryable = _userIdentities.BuildMock();

        _userManagerMock.Setup(x => x.Users)
                        .Returns(queryable);

        // Act
        var token = await _tokenService.RefreshToken(userName, tokens);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Theory]
    [InlineData("user1")]
    [InlineData("user2")]
    [InlineData("user3")]
    public void GetPrincipal_ShouldReturnValidPrincipal(string userName)
    {
        // Arrange
        var token = _tokenService.GenerateAccessToken(userName);

        // Act
        var principal = _tokenService.GetPrincipalFromExpiredToken(token);

        // Assert
        Assert.NotNull(principal);
        Assert.Equal(userName, principal.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}