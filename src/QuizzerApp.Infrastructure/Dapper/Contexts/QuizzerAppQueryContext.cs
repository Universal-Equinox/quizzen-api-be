using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QuizzerApp.Infrastructure.Dapper.Contexts;

public class QuizzerAppQueryContext
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public QuizzerAppQueryContext(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetSection("SqlServer")["connectionString"];
    }

    public IDbConnection Connect() => new SqlConnection(_connectionString);

}
