using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizzenApp.Domain.Entities.UserAggregate;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Infrastructure.EFCore.Contexts;
using QuizzerApp.Infrastructure.Persistence;

namespace QuizzerApp.Infrastructure.EFCore;

public static class Extensions
{
    public static void UseSqlServer(this IServiceCollection services, IConfiguration config)
    {
        var sqlServerOptions = config.GetSection("SqlServer");
        services.AddDbContext<QuizzerAppContext>(context => context.UseSqlServer(sqlServerOptions["ConnectionString"]));
    }

    public static void UseSqlLite(this IServiceCollection services)
    {
        // services.AddDbContext<QuizzerAppContext>(context => context.UseSqlite("Data Source=App.db"));

    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
                        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<QuizzerAppContext>()
            .AddDefaultTokenProviders();
            
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }
}
