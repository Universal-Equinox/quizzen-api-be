using MediatR;
using Microsoft.EntityFrameworkCore;
using QuizzenApp.Domain.Entities.QuestionAggregate.ValueObjects;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Application.Dtos.Exam;
using QuizzerApp.Application.Dtos.Question;
using QuizzerApp.Application.Dtos.User;

namespace QuizzerApp.Application.Features.Queries.Question.ReadQuestionById;

public class ReadQuestionQueryHandler : IRequestHandler<ReadQuestionByIdQuery, QuestionDto>
{
    private readonly IRepositoryManager _manager;

    public ReadQuestionQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<QuestionDto> Handle(ReadQuestionByIdQuery request, CancellationToken cancellationToken)
    {
    
        var query = _manager.Question.GetQueriable();

        var dbQuestion = await query.Include(q => q.User)
                           .Include(q => q.Exam)
                           .Include(q => q.Subject)
                           .Include(q => q.Topic)
                           .FirstOrDefaultAsync(q => q.Id == new QuestionId(request.QuestionId), cancellationToken);



        QuestionDto res = new(Id: dbQuestion.Id,
                Title: dbQuestion.Title,
                Description: dbQuestion.Description,
                Status: dbQuestion.Status.ToString(),
                User: new UserDto(dbQuestion.UserId,
                                  dbQuestion.User.UserName,
                                  dbQuestion.User.FirstName,
                                  dbQuestion.User.LastName),
                Tags: new ExamDto(dbQuestion.Exam.Name, dbQuestion.Subject.Name, dbQuestion.Topic.Name),
                CreatedDate: dbQuestion.CreatedDate);

        return res;
    }
}
