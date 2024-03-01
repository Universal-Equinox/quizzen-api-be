using System.Text;
using Dapper;
using MediatR;
using QuizzerApp.Application.Abstacts;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Application.Dtos.Question;

namespace QuizzerApp.Application.Features.Queries.Question.ReadQuestions;

public class ReadQuestionQueryHandler : IRequestHandler<ReadQuestionQuery, List<QuestionResponse>>
{
    private readonly ISqlConnectionFactory _connection;

    public ReadQuestionQueryHandler(ISqlConnectionFactory connection)
    {
        _connection = connection;
    }

    public async Task<List<QuestionResponse>> Handle(ReadQuestionQuery request, CancellationToken cancellationToken)
    {
        var whereClause = new StringBuilder("WHERE 1=1");

        if (!string.IsNullOrEmpty(request.Exam))
        {
            whereClause.Append(" AND e.Name = @Exam ");
        }

        if (!string.IsNullOrEmpty(request.Subject))
        {
            whereClause.Append(" AND s.Name = @Subject ");
        }

        if (!string.IsNullOrEmpty(request.Topic))
        {
            whereClause.Append(" AND t.Name = @Topic ");
        }

        if (!string.IsNullOrEmpty(request.UserId))
        {
            whereClause.Append(" AND u.Id = @UserId ");
        }

        var query = new StringBuilder();

        query.AppendFormat(@"
                    SELECT 
                        q.Id,
                        q.Title,
                        q.CreatedDate,
                        q.Status,
                        q.Description,
                        COUNT(a.QuestionId) AS AnswerCount,
                        COUNT(qv.QuestionId) AS VoteCount,
                        u.Id ,
                        u.UserName AS UserName,
                        u.FirstName AS FirstName,
                        u.LastName AS LastName,
                        u.ProfileImg AS ProfileImg,
                        e.Name AS Exam,
                        s.Name AS Subject,
                        t.Name AS Topic
                    FROM Questions q
                    JOIN AspNetUsers u ON q.UserId = u.Id
                    JOIN Exams e ON q.ExamId = e.Id
                    JOIN Subjects s ON q.SubjectId = s.Id
                    JOIN Topics t ON q.TopicId = t.Id
                    LEFT JOIN Answers a ON q.Id = a.QuestionId 
                    LEFT JOIN QuestionVotes qv ON q.Id = qv.QuestionId
                    {0}
                     GROUP BY 
                    q.Id, q.Title, q.CreatedDate, q.Status, q.Description, 
                    u.Id, u.UserName, u.FirstName, u.LastName, u.ProfileImg, 
                    e.Name, s.Name, t.Name
                    ORDER BY q.CreatedDate DESC;
                    SELECT qi.QuestionId AS QId, qi.ImgPath AS Url FROM QuestionImages qi
                    ", whereClause);


        using var conn = _connection.Connect();

        using var multi = await conn.QueryMultipleAsync(query.ToString(), request);

        var questionDtos = multi.Read<QuestionResponse, UserDto, TagsDto, QuestionResponse>(
            (question, user, exam) =>
            {
                question.User = user;
                question.Tags = exam;


                return question;
            },
            splitOn: "Id,Id,Exam");

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