using System.Data;

namespace QuizzerApp.Application.Abstacts;

public interface ISqlConnectionFactory
{
    IDbConnection Connect();
}
