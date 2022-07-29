using System;
using System.Text;
using AutoMapper;
using ChatApp.Infrastructure.Identity.Entities;
using ChatApp.Infrastructure.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace ChatApp.Infrastructure.Tests;

public static class UnitTestHelpers
{
    public static Mock<UserManager<UserIdentity>> CreateUserManagerMock()
    {
        var userStoreMock = new Mock<IUserStore<UserIdentity>>();
        var userManagerMock = new Mock<UserManager<UserIdentity>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        return userManagerMock;
    }

    public static Mock<SignInManager<UserIdentity>> CreateSignInManagerMock()
    {
        var userManagerMock = CreateUserManagerMock();
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<UserIdentity>>();

        var signInManagerMock = new Mock<SignInManager<UserIdentity>>(userManagerMock.Object, contextAccessorMock.Object, claimsFactoryMock.Object,
            null, null, null, null);

        return signInManagerMock;
    }

    public static TokenValidationParameters CreateTokenValidationParameters(string key)
    {
        return new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuerSigningKey = true
        };
    }

    public static IMapper CreateMapperProfile()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MapperProfile());
        });

        return config.CreateMapper();
    }
}