using MediatR;
using QuizzenApp.Shared.Dto;
using QuizzerApp.Application.Dtos.Question;

namespace QuizzerApp.Application.Features.Queries.Question.ReadQuestionById;

public record ReadQuestionByIdQuery(Guid QuestionId) : IRequest<QuestionResponse>;

