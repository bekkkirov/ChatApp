using ChatApp.Infrastructure.Identity;
using ChatApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.WebUI.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddDatabaseContexts(configuration);
    }

    private static void AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("IdentityDb")));

        services.AddDbContext<ChatContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("MainDb")));
    }
}