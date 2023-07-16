
using AppMinimalApi.EF;
using AppMinimalApi.Endpoints;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;

namespace AppMinimalApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSqlServer<AppDbContext>(builder.Configuration["ConnectionStrings:appDb"]);
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        //Configure Serilog
        builder.Host.UseSerilog((ctx, lc) => lc
                    .WriteTo.Console(LogEventLevel.Debug)
                    .WriteTo.File("log.txt", LogEventLevel.Warning,
                     rollingInterval: RollingInterval.Day));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.ConfigureAuthEndpoints();

        app.Run();
    }
}