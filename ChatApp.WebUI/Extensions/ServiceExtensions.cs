using System.Text;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Persistence;
using ChatApp.Application.Common.Settings;
using ChatApp.Infrastructure.Identity;
using ChatApp.Infrastructure.Identity.Entities;
using ChatApp.Infrastructure.Mapping;
using ChatApp.Infrastructure.Persistence;
using ChatApp.Infrastructure.Persistence.Repositories;
using ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.WebUI.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddDatabaseContexts(configuration);
        services.AddRepositories();
        services.AddAutoMapper(typeof(MapperProfile).Assembly);
        services.AddApplicationServices();
        services.AddJwt(configuration);
        services.AddIdentity();
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

    private static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IIdentityService, IdentityService>();
    }

    private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenSettings>(configuration.GetSection("Jwt"));

        var tokenValidationParams = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateIssuerSigningKey = true
        };

        services.AddSingleton<TokenValidationParameters>(tokenValidationParams);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.TokenValidationParameters = tokenValidationParams;
                });
    }

    private static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<UserIdentity>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 5;
                })
                .AddRoles<UserRole>()
                .AddRoleManager<RoleManager<UserRole>>()
                .AddSignInManager<SignInManager<UserIdentity>>()
                .AddRoleValidator<RoleValidator<UserRole>>()
                .AddEntityFrameworkStores<IdentityContext>();

    }
}