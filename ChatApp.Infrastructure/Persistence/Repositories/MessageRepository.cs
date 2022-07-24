using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Persistence.Repositories;

///<inheritdoc cref="IMessageRepository"/>
public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    /// <summary>
    /// Creates an instance of the message repository.
    /// </summary>
    public MessageRepository(ChatContext context) : base(context)
    {
    }
}