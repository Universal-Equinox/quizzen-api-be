using MediatR;
using QuizzenApp.Shared.Dto;
using QuizzerApp.Application.Dtos.Answer;

namespace QuizzerApp.Application.Features.Queries.Answer.ReadAnswers;

public record ReadAnswersQuery(string? QuestionId, string? userId = null) : IRequest<List<AnswerReponse>>;
