using Dapper;
using Microsoft.AspNetCore.Mvc;
using QuizzerApp.Application.Dtos.Question;
using QuizzerApp.Infrastructure.Dapper.Contexts;

namespace QuizzerApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class TestController : BaseController
{
    private readonly QuizzerAppQueryContext _context;

    public TestController(QuizzerAppQueryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Test()
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

        using var conn = _context.Connect();

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


        // var questionDtos = await conn.QueryAsync<QuestionResponse, UserDto, TagsDto, QuestionResponse>(query, (question, user, exam) =>
        //     {
        //         question.User = user;
        //         question.Tags = exam;

        //         return question;
        //     },
        //     splitOn: "Id,Id,Exam");








        return Ok(questionDtos);
    }
}
