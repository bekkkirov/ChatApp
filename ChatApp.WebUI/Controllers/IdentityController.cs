using System.Security.Claims;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.WebUI.Controllers;

[ApiController]
[Route("api/auth")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public IdentityController(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("sign-in")]
    public async Task<ActionResult<TokensModel>> SignInAsync(SignInModel signInData)
    {
        var tokens = await _identityService.SignInAsync(signInData);

        return Ok(tokens);
    }

    [HttpPost]
    [Route("sign-up")]
    public async Task<ActionResult<TokensModel>> SignUpAsync(SignUpModel signUpData)
    {
        var tokens = await _identityService.SignUpAsync(signUpData);

        return Ok(tokens);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<string>> RefreshToken(string userName, TokensModel tokens)
    {
        var accessToken = await _tokenService.RefreshToken(userName, tokens);

        return Ok(accessToken);
    }
}