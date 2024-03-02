// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using QuizzenApp.Domain.Entities.AnswerAggregate.ValueObjects;
// using QuizzerApp.Application.Common.Interfaces;
// using QuizzerApp.Application.Dtos.Answer;
// using QuizzerApp.Application.Dtos.Image;
// using QuizzerApp.Application.Dtos.User;

// namespace QuizzerApp.Application.Features.Queries.Answer.ReadAnswerById;

// public class ReadAnswerByIdQueryHandler : IRequestHandler<ReadAnswerByIdQuery, AnswerDto>
// {
//     private readonly IRepositoryManager _manager;

//     public ReadAnswerByIdQueryHandler(IRepositoryManager manager)
//     {
//         _manager = manager;
//     }

//     public async Task<AnswerDto> Handle(ReadAnswerByIdQuery request, CancellationToken cancellationToken)
//     {

//         var query = _manager.Answer.GetQueriable();

//         var answer = await query.Include(a => a.User)
//                                 .FirstOrDefaultAsync(a => a.Id == new AnswerId(request.AnswerId), cancellationToken);

//         AnswerDto res = new(
//             Id: answer.Id,
//             Text: answer.Text,
//             Status: answer.Status,
//             User: new UserDto(answer.User.Id,
//                               answer.User.UserName,
//                               answer.User.FirstName,
//                               answer.User.LastName),
//             QuestionId: answer.QuestionId.Value,
//             Images: _manager.Photo.GetDbAnswerImgPaths(answer.Id)
//                                   .Select(img => new ImageDto(img.ImgPath))
//                                   .ToList(),
//             CreatedDate: answer.CreatedDate,
//             UpdatedDate: answer.UpdatedDate
//         );

//         return res;
//     }
// }
