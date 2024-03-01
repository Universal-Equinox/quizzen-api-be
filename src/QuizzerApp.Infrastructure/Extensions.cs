using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Infrastructure.Dapper;
using QuizzerApp.Infrastructure.EFCore;
using QuizzerApp.Infrastructure.Persistence;

namespace QuizzerApp.Infrastructure;

public static class Extensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.UseSqlServer(config);
        // services.UseSqlLite();
        // services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.ConfigureIdentity();

        services.ConfigureQueryDbContext();


    }
}
