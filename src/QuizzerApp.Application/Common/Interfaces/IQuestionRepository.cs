using QuizzenApp.Domain.Entities.QuestionAggregate;
using QuizzenApp.Domain.Entities.QuestionAggregate.ValueObjects;

namespace QuizzerApp.Application.Common.Interfaces;

public interface IQuestionRepository
{
    Task<List<Question>> GetAllAsync();
    Task<List<Question>> GetAllByUserIdAsync(string userId);
    IQueryable<Question> GetQueriable();
    Task<Question> GetAsync(Guid id);
    Task CreateAsync(Question question);
    void UpdateAsync(Question question);
    void DeleteAsync(Question question);
    int GetQuestionVoteCount(Guid id);

}
