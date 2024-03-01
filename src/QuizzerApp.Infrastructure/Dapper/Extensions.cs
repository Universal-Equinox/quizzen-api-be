using Microsoft.Extensions.DependencyInjection;
using QuizzerApp.Application.Abstacts;
using QuizzerApp.Infrastructure.Dapper.Connections;
using QuizzerApp.Infrastructure.Dapper.Contexts;

namespace QuizzerApp.Infrastructure.Dapper;

public static class Extensions
{

    public static void ConfigureQueryDbContext(this IServiceCollection services)
    {
        services.AddScoped<QuizzerAppQueryContext>();
        services.AddScoped<ISqlConnectionFactory, SqlServerConnectionFactory>();
    }

}
