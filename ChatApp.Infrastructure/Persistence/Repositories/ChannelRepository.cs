using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Persistence.Repositories;

///<inheritdoc cref="IChannelRepository"/>
public class ChannelRepository : BaseRepository<Channel>, IChannelRepository
{
    /// <summary>
    /// Creates an instance of the channel repository.
    /// </summary>
    public ChannelRepository(ChatContext context) : base(context)
    {
    }
}