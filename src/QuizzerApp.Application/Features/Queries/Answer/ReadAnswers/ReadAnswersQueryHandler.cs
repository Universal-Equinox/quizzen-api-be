// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using QuizzenApp.Domain.Entities.QuestionAggregate.ValueObjects;
// using QuizzerApp.Application.Common.Interfaces;
// using QuizzerApp.Application.Dtos.Answer;
// using QuizzerApp.Application.Dtos.Image;
// using QuizzerApp.Application.Dtos.User;

// namespace QuizzerApp.Application.Features.Queries.Answer.ReadAnswers;

// public class ReadAnswersQueryHandler : IRequestHandler<ReadAnswersQuery, List<AnswerDto>>
// {
//     private readonly IRepositoryManager _manager;

//     public ReadAnswersQueryHandler(IRepositoryManager manager)
//     {
//         _manager = manager;
//     }

//     public async Task<List<AnswerDto>> Handle(ReadAnswersQuery request, CancellationToken cancellationToken)
//     {
//         var query = _manager.Answer.GetQueriable();

//         if (!string.IsNullOrEmpty(request.QuestionId))
//             query = query.Where(a => a.QuestionId == new QuestionId(new Guid(request.QuestionId)));

//         if (!string.IsNullOrEmpty(request.userId))
//         {
//             query = query.Where(a => a.UserId == request.userId);

//         }
//         var answers = await query.Include(a => a.User).ToListAsync(cancellationToken);


//         var res = answers.Select(a => new AnswerDto(
//             Id: a.Id,
//             Text: a.Text,
//             Status: a.Status,
//             User: new UserDto(a.UserId, a.User.UserName, a.User.FirstName, a.User.LastName,
//             profileImg: a.User.ProfileImg),
//             QuestionId: a.QuestionId.Value,
//             Images: _manager.Photo.GetDbAnswerImgPaths(a.Id.Value)
//                                   .Select(img => new ImageDto(Url: img.ImgPath))
//                                   .ToList(),
//             CreatedDate: a.CreatedDate,
//             UpdatedDate: a.UpdatedDate
//         )).ToList();

//         return new List<AnswerDto>(res);
//     }
// }
