using ChatApp.Application.Common.Persistence;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Persistence.Repositories;

///<inheritdoc cref="IImageRepository"/>
public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    /// <summary>
    /// Creates an instance of the image repository.
    /// </summary>
    public ImageRepository(ChatContext context) : base(context)
    {
    }
}