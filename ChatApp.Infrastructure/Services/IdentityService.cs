using AutoMapper;
using ChatApp.Application.Common.Exceptions;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models.Authorization;
using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Identity.Entities;
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
        var user = await _userManager.FindByNameAsync(signInData.UserName);

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

    /// <summary>
    /// Generates a refresh token for the specified user and adds it to the database.
    /// </summary>
    /// <returns>Refresh token.</returns>
    private async Task<string> GenerateRefreshTokenAsync(UserIdentity user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = new RefreshToken()
        {
            Token = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(31),
        };

        await _userManager.UpdateAsync(user);

        return refreshToken;
    }
}