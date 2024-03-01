using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuizzerApp.Application.Abstacts;

namespace QuizzerApp.Infrastructure.Dapper.Connections;

public class SqlServerConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public SqlServerConnectionFactory(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetSection("SqlServer")["connectionString"];
    }

    public IDbConnection Connect() => new SqlConnection(_connectionString);
}
