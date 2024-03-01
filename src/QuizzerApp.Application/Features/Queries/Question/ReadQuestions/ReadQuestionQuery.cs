using MediatR;
using QuizzenApp.Shared.Dto;
using QuizzerApp.Application.Dtos.Question;

namespace QuizzerApp.Application.Features.Queries.Question.ReadQuestions;

public record ReadQuestionQuery(string? Exam = null, string? Subject = null, string? Topic = null, string? UserId = null) : IRequest<List<QuestionResponse>>;
