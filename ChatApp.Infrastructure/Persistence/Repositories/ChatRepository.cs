using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Persistence.Repositories;

///<inheritdoc cref="IChatRepository"/>
public class ChatRepository : BaseRepository<Chat>, IChatRepository
{
    /// <summary>
    /// Creates an instance of the chat repository.
    /// </summary>
    public ChatRepository(ChatContext context) : base(context)
    {
    }
}