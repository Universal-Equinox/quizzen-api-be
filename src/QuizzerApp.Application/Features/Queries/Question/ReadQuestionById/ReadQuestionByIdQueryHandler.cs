using MediatR;
using Microsoft.EntityFrameworkCore;
using QuizzenApp.Domain.Entities.QuestionAggregate.ValueObjects;
using QuizzenApp.Shared.Dto;
using QuizzenApp.Shared.Exceptions;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Application.Dtos.Exam;
using QuizzerApp.Application.Dtos.Image;
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
                           .Include(q => q.QuestionVotes)
                           .Include(q => q.Answers)
                           .FirstOrDefaultAsync(q => q.Id == new QuestionId(request.QuestionId), cancellationToken);

        if (dbQuestion is null) throw new NotFoundException("Question", request.QuestionId.ToString());


        QuestionDto res = new(Id: dbQuestion.Id,
                Title: dbQuestion.Title,
                Description: dbQuestion.Description,
                Status: dbQuestion.Status.ToString(),
                User: new UserDto(dbQuestion.UserId,
                                  dbQuestion.User.UserName,
                                  dbQuestion.User.FirstName,
                                  dbQuestion.User.LastName,
                                  profileImg: dbQuestion.User.ProfileImg),
                Tags: new ExamDto(dbQuestion.Exam.Name, dbQuestion.Subject.Name, dbQuestion.Topic.Name),
                Images: _manager.Photo.GetDbQuestionImgPaths(dbQuestion.Id.Value)
                                      .Select(img => new ImageDto(img.ImgPath))
                                      .ToList(),
                CreatedDate: dbQuestion.CreatedDate,
                VoteCount: dbQuestion.QuestionVotes.Select(qv => qv.QuestionId == dbQuestion.Id).Count(),
                AnswerCount: dbQuestion.Answers.Select(a => a.QuestionId == dbQuestion.Id).Count()
                );

        return res;
    }
}
