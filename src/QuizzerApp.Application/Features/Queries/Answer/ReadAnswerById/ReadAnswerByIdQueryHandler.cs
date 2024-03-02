using System.Text;
using Dapper;
using MediatR;
using QuizzerApp.Application.Abstacts;
using QuizzerApp.Application.Dtos.Answer;


namespace QuizzerApp.Application.Features.Queries.Answer.ReadAnswerById;

public class ReadAnswerByIdQueryHandler : IRequestHandler<ReadAnswerByIdQuery, AnswerReponse>
{
    private readonly ISqlConnectionFactory _connection;

    public ReadAnswerByIdQueryHandler(ISqlConnectionFactory connection)
    {

        _connection = connection;
    }

    public async Task<AnswerReponse?> Handle(ReadAnswerByIdQuery request, CancellationToken cancellationToken)
    {
        var whereClause = new StringBuilder("WHERE 1=1");

        if (!Equals(request.AnswerId, null))
        {
            whereClause.Append(" AND a.Id = @AnswerId ");
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
                    WHERE a.Id = @AnswerId
                     GROUP BY 
                    a.Id, a.Text, a.Status, a.CreatedDate, a.QuestionId,
                    u.Id, u.UserName, u.FirstName, u.LastName, u.ProfileImg
                    ORDER BY a.CreatedDate DESC;
                    SELECT ai.AnswerId AS QId, ai.ImgPath AS Url FROM AnswerImages ai
                    ");

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


        return questionDtos.FirstOrDefault();


    }
}
