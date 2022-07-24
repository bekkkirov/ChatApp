using ChatApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Identity.Extensions;

public static class UserManagerExtensions
{
    public static async Task<UserIdentity> FindByNameWithRefreshTokenAsync(this UserManager<UserIdentity> userManager, string userName)
    {
        return await userManager.Users.Include(u => u.RefreshToken)
                                      .FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
    }
}