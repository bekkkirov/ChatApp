using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Repositories;

///<inheritdoc cref="IUserRepository"/>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    /// <summary>
    /// Creates an instance of the user repository.
    /// </summary>
    public UserRepository(ChatContext context) : base(context)
    {

    }

    public async Task<User> GetByUserNameAsync(string userName)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<User> GetUserWithProfileImageAsync(string userName)
    {
        return await _dbSet.Include(u => u.Profile)
                           .ThenInclude(p => p.ProfileImage)
                           .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<User> GetUserWithProfileAndSettingsAsync(string userName)
    {
        return await _dbSet.Include(u => u.Profile)
                           .ThenInclude(p => p.ProfileImage)
                           .Include(u => u.Settings)
                           .ThenInclude(s => s.BackgroundImage)
                           .FirstOrDefaultAsync(u => u.UserName == userName);
    }
}