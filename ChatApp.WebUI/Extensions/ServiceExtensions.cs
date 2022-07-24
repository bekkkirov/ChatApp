using ChatApp.Application.Common.Persistence;
using ChatApp.Infrastructure.Identity;
using ChatApp.Infrastructure.Persistence;
using ChatApp.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.WebUI.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddDatabaseContexts(configuration);
        services.AddRepositories();
    }

    private static void AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("IdentityDb")));

        services.AddDbContext<ChatContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("MainDb")));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}