using ChatApp.Application.Common.Validation.Authorization;
using ChatApp.WebUI.Extensions;
using FluentValidation.AspNetCore;

namespace ChatApp.WebUI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCorsPolicy();

        builder.Services.AddControllers()
                        .AddFluentValidation(opt =>
                        {
                            opt.RegisterValidatorsFromAssembly(typeof(TokensModelValidator).Assembly);
                        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("DefaultPolicy");
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}