using System.Security.Claims;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Common.Models.Image;
using ChatApp.Application.Common.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("current/edit/profileImage")]
    public async Task<ActionResult<ImageModel>> SetProfileImage(IFormFile image)
    {
        var currentUser = GetCurrentUser();

        var created = await _userService.SetProfileImageAsync(currentUser, image);

        return Ok(created);
    }

    [HttpPost]
    [Route("current/edit/{fingerPrint}/backgroundImage")]
    public async Task<ActionResult<ImageModel>> SetBackgroundImage(IFormFile image, string fingerPrint)
    {
        var currentUser = GetCurrentUser();

        var created = await _userService.SetBackgroundImageAsync(currentUser, fingerPrint, image);

        return Ok(created);
    }

    [HttpGet]
    [Route("current/{fingerPrint}")]
    public async Task<ActionResult<CurrentUserModel>> GetCurrentUserInfo(string fingerPrint)
    {
        var currentUser = GetCurrentUser();

        var userInfo = await _userService.GetUserInfoAsync(currentUser, fingerPrint);

        return Ok(userInfo);
    }

    [HttpPut]
    [Route("current/update")]
    public async Task<ActionResult> UpdateUserInfo(UserUpdateModel updateData)
    {
        var currentUser = GetCurrentUser();

        await _userService.UpdateUserInfoAsync(currentUser, updateData);

        return NoContent();
    }

    private string GetCurrentUser()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}