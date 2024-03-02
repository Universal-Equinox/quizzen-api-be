using MediatR;
using QuizzenApp.Shared.Dto;
using QuizzerApp.Application.Dtos.Answer;

namespace QuizzerApp.Application.Features.Queries.Answer.ReadAnswerById;

public record ReadAnswerByIdQuery(Guid AnswerId) : IRequest<AnswerReponse>;
