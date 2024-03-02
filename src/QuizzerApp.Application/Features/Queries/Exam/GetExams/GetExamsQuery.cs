using System.Text;
using Dapper;
using MediatR;
using QuizzerApp.Application.Abstacts;
using QuizzerApp.Application.Dtos.Exam;

namespace QuizzerApp.Application.Features.Queries.Exam.GetExams;

public record GetExamsQuery(string? ExamId, string? SubjectId) : IRequest<List<ExamResponse>>;

public class GetExamsQueryHandler : IRequestHandler<GetExamsQuery, List<ExamResponse>>
{
    private readonly ISqlConnectionFactory _connection;

    public GetExamsQueryHandler(ISqlConnectionFactory connection)
    {
        _connection = connection;
    }

    public async Task<List<ExamResponse>> Handle(GetExamsQuery request, CancellationToken cancellationToken)
    {
        var reqParamTableName = "Exams";

        var whereClause = "WHERE 1=1";

        if (!string.IsNullOrEmpty(request.ExamId))
        {
            reqParamTableName = "Subjects";
            whereClause = "WHERE ExamId=@ExamId";
        }

        if (!string.IsNullOrEmpty(request.SubjectId))
        {
            reqParamTableName = "Topics";
            whereClause = "WHERE SubjectId=@SubjectId";
        }


        var query = new StringBuilder();

        query.AppendFormat(@"
            SELECT
                Id,
                Name
            FROM {0}
            {1}
        ", reqParamTableName, whereClause);

        using var conn = _connection.Connect();

        var res = await conn.QueryAsync<ExamResponse>(query.ToString(), request);

        return res.ToList();

    }
}
