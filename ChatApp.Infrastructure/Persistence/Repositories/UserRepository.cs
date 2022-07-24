using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;

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
}