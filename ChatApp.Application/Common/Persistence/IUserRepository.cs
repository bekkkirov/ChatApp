using ChatApp.Domain.Entities;

namespace ChatApp.Application.Common.Persistence;

/// <summary>
/// Represents a user repository.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Gets user with the specified username;
    /// </summary>
    Task<User> GetByUserNameAsync(string userName);

    /// <summary>
    /// Gets user data including profile image.
    /// </summary>
    Task<User> GetUserWithProfileImageAsync(string userName);

    /// <summary>
    /// Gets user data including settings and profile.
    /// </summary>
    Task<User> GetUserWithProfileAndSettingsAsync(string userName);
}