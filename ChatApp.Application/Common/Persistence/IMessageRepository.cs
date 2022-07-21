using ChatApp.Domain.Entities;

namespace ChatApp.Application.Common.Persistence;

/// <summary>
/// Represents a message repository.
/// </summary>
public interface IMessageRepository : IRepository<Message>
{
    
}