using Microsoft.EntityFrameworkCore;
using QuizzenApp.Domain.Entities.AnswerAggregate;
using QuizzenApp.Domain.Entities.AnswerAggregate.ValueObjects;
using QuizzenApp.Domain.Entities.QuestionAggregate.ValueObjects;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Infrastructure.Contracts;
using QuizzerApp.Infrastructure.EFCore.Contexts;

namespace QuizzerApp.Infrastructure.Persistence;

public class AnswerRepository: RepositoryBase<Answer>, IAnswerRepository
{
    public AnswerRepository(QuizzerAppContext context) : base(context)
    {
        
    }
    public async Task CreateAsync(Answer answer) => Create(answer);

    public void DeleteAsync(Answer answer)
    {
        throw new NotImplementedException();
    }

    public Task<List<Answer>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Answer>> GetAllByQuestionIdAsync(Guid questionId) => FindAll(false).OrderBy(a => a.QuestionId == new QuestionId(questionId)).ToList();

    public async Task<List<Answer>> GetAllByUserIdAsync(string userId) => FindAll(false).OrderBy(a => a.UserId == userId).ToList();

    public async Task<Answer> GetAsync(Guid id) => await FindByCondition(q => q.Id == new AnswerId(id), false).FirstOrDefaultAsync();

    public IQueryable<Answer> GetQueriable() => Queriable();

    public void UpdateAsync(Answer answer)
    {
        throw new NotImplementedException();
    }
}
