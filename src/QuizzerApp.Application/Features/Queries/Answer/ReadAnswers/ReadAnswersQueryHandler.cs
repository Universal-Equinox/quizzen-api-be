using System.Text;
using Dapper;
using MediatR;
using QuizzerApp.Application.Abstacts;
using QuizzerApp.Application.Dtos.Answer;


namespace QuizzerApp.Application.Features.Queries.Answer.ReadAnswers;

public class ReadAnswersQueryHandler : IRequestHandler<ReadAnswersQuery, List<AnswerReponse>>
{
    private readonly ISqlConnectionFactory _connection;

    public ReadAnswersQueryHandler(ISqlConnectionFactory connection)
    {
        _connection = connection;
    }

    public async Task<List<AnswerReponse>> Handle(ReadAnswersQuery request, CancellationToken cancellationToken)
    {
        var whereClause = new StringBuilder("WHERE 1=1");

        if (!string.IsNullOrEmpty(request.QuestionId))
        {
            whereClause.Append(" AND a.QuestionId = @QuestionId ");
        }

        if (!string.IsNullOrEmpty(request.userId))
        {
            whereClause.Append(" AND a.UserId = @UserId ");
        }


        var query = new StringBuilder();

        query.AppendFormat(@"
                    SELECT 
                        a.Id,
                        a.Text,
                        a.Status,
                        a.CreatedDate,
                        a.QuestionId,
                        COUNT(qv.AnswerId) AS VoteCount,
                        u.Id,
                        u.UserName AS UserName,
                        u.FirstName AS FirstName,
                        u.LastName AS LastName,
                        u.ProfileImg AS ProfileImg
                    FROM Answers a
                    JOIN AspNetUsers u ON a.UserId = u.Id
                    LEFT JOIN AnswerVotes qv ON a.Id = qv.AnswerId
                    {0}
                     GROUP BY 
                    a.Id, a.Text, a.Status, a.CreatedDate, a.QuestionId,
                    u.Id, u.UserName, u.FirstName, u.LastName, u.ProfileImg
                    ORDER BY a.CreatedDate DESC;
                    SELECT ai.AnswerId AS QId, ai.ImgPath AS Url FROM AnswerImages ai
                    ", whereClause);

        using var conn = _connection.Connect();

        using var multi = await conn.QueryMultipleAsync(query.ToString(), request);

        var questionDtos = multi.Read<AnswerReponse, UserDto, AnswerReponse>(
            (answer, user) =>
            {
                answer.User = user;


                return answer;
            },
            splitOn: "Id,Id");

        var images = multi.Read<ImageDto>().ToList();

        images.ForEach(i =>
        {
            var question = questionDtos.FirstOrDefault(q => q.Id == i.QId);
            if (question is not null)
                question.Images.Add(i.Url);
        });


        return questionDtos.ToList();


    }
}
