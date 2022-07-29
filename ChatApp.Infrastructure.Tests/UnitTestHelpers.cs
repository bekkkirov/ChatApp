using System;
using System.Text;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Identity;
using ChatApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
}