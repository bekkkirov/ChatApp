using ChatApp.Domain.Entities;

namespace ChatApp.Application.Common.Persistence;

/// <summary>
/// Represents a chat repository.
/// </summary>
public interface IChatRepository : IRepository<Chat>
{
    
}