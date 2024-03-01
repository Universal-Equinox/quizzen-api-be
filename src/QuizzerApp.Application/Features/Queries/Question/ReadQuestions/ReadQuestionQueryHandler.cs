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
 string query = @"
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
                        GROUP BY 
                            q.Id, q.Title, q.CreatedDate, q.Status, q.Description, 
                            u.Id, u.UserName, u.FirstName, u.LastName, u.ProfileImg, 
                            e.Name, s.Name, t.Name;
                         SELECT qi.QuestionId AS QId, qi.ImgPath AS Url FROM QuestionImages qi;
    ";

        using var conn = _connection.Connect();

        using var multi = await conn.QueryMultipleAsync(query);

        var questionDtos = multi.Read<QuestionResponse, UserDto, TagsDto, QuestionResponse>(
            (question, user, exam) =>
            {
                question.User = user;
                question.Tags = exam;
                

                return question;
            },
            splitOn: "Id,Id,Exam");

        var images = multi.Read<ImageDto>().ToList();

        foreach (var image in images)
        {
            var question = questionDtos.FirstOrDefault(q => q.Id == image.QId);
            if (question != null)
            {
                question.Images.Add(image.Url);
            }
        }


        if (!string.IsNullOrEmpty(request.Exam))
            questionDtos = questionDtos.Where(q => string.Equals(q.Tags.Exam, request.Exam)) ?? questionDtos;

        if (!string.IsNullOrEmpty(request.Subject))
            questionDtos = questionDtos.Where(q => string.Equals(q.Tags.Subject, request.Subject));

        if (!string.IsNullOrEmpty(request.Topic))
            questionDtos = questionDtos.Where(q => string.Equals(q.Tags.Topic, request.Topic));

        if (!string.IsNullOrEmpty(request.UserId))
            questionDtos = questionDtos.Where(q => string.Equals(q.User.Id, request.UserId));


        return questionDtos.ToList();

    }
}