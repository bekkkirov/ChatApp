using ChatApp.Application.Common.Persistence;

namespace ChatApp.Infrastructure.Persistence.Repositories;

///<inheritdoc cref="IUnitOfWork"/>
public class UnitOfWork : IUnitOfWork
{
    private readonly ChatContext _context;

    public IUserRepository UserRepository { get; }

    public IImageRepository ImageRepository { get; }

    public IMessageRepository MessageRepository { get; }

    public IChatRepository ChatRepository { get; }

    public IChannelRepository ChannelRepository { get; }

    /// <summary>
    /// Creates an instance of the unit of work.
    /// </summary>
    public UnitOfWork(IUserRepository userRepository,
                      IImageRepository imageRepository,
                      IMessageRepository messageRepository,
                      IChatRepository chatRepository,
                      IChannelRepository channelRepository,
                      ChatContext context)
    {
        UserRepository = userRepository;
        ImageRepository = imageRepository;
        MessageRepository = messageRepository;
        ChatRepository = chatRepository;
        ChannelRepository = channelRepository;
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}