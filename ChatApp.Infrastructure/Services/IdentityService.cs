﻿using AutoMapper;
using ChatApp.Application.Common.Exceptions;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models.Authorization;
using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Identity.Entities;
using ChatApp.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Infrastructure.Services;

///<inheritdoc cref="IIdentityService"/>
public class IdentityService : IIdentityService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates a new instance of the Identity service.
    /// </summary>
    public IdentityService(UserManager<UserIdentity> userManager,
                           SignInManager<UserIdentity> signInManager,
                           ITokenService tokenService,
                           IUnitOfWork unitOfWork,
                           IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TokensModel> SignInAsync(SignInModel signInData)
    {
        var user = await _userManager.FindByNameWithRefreshTokenAsync(signInData.UserName);

        if (user is null)
        {
            throw new IdentityException("User with specified username doesn't exist.");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, signInData.Password, false);

        if (!signInResult.Succeeded)
        {
            throw new IdentityException("Invalid password.");
        }

        var accessToken = _tokenService.GenerateAccessToken(user.UserName);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        return new TokensModel() {AccessToken = accessToken, RefreshToken = refreshToken};
    }

    public async Task<TokensModel> SignUpAsync(SignUpModel signUpData)
    {
        var userIdentity = new UserIdentity() {UserName = signUpData.UserName, Email = signUpData.Email};

        var signUpResult = await _userManager.CreateAsync(userIdentity, signUpData.Password);

        if (!signUpResult.Succeeded)
        {
            throw new IdentityException(signUpResult.Errors.First().Description);
        }

        var user = _mapper.Map<User>(signUpData);

        _unitOfWork.UserRepository.Add(user);
        await _unitOfWork.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(user.UserName);
        var refreshToken = await GenerateRefreshTokenAsync(userIdentity);

        return new TokensModel() {AccessToken = accessToken, RefreshToken = refreshToken};
    }

    public async Task ChangeEmailAsync(string userName, string newEmail)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user.Email != newEmail)
        {
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

            if (!result.Succeeded)
            {
                throw new IdentityException(result.Errors.FirstOrDefault()
                                                  ?.Description);
            }
        }
    }

    public async Task ChangePasswordAsync(string userName, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (await _userManager.CheckPasswordAsync(user, newPassword))
        {
            throw new IdentityException("New password can't be the same as the old one.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors.FirstOrDefault()?.Description);
        }
    }

    /// <summary>
    /// Generates a refresh token for the specified user and adds it to the database.
    /// </summary>
    /// <returns>Refresh token.</returns>
    private async Task<string> GenerateRefreshTokenAsync(UserIdentity user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        if (user.RefreshToken is null)
        {
            user.RefreshToken = new RefreshToken()
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(31),
            };
        }

        else
        {
            user.RefreshToken.Token = refreshToken;
            user.RefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(31);
            user.RefreshToken.IsUsed = false;
        }

        await _userManager.UpdateAsync(user);

        return refreshToken;
    }
}