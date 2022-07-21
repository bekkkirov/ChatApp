namespace ChatApp.Application.Common.Persistence;

/// <summary>
/// Represents a unit of work.
/// </summary>
public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }

    public IImageRepository ImageRepository { get; }

    public IMessageRepository MessageRepository { get; }

    public IChatRepository ChatRepository { get; }

    public IChannelRepository ChannelRepository { get; }

    Task SaveChangesAsync();
}