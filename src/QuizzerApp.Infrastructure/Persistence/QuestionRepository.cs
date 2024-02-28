using Microsoft.EntityFrameworkCore;
using QuizzenApp.Domain.Entities.QuestionAggregate;
using QuizzenApp.Domain.Entities.QuestionAggregate.ValueObjects;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Infrastructure.Contracts;
using QuizzerApp.Infrastructure.EFCore.Contexts;

namespace QuizzerApp.Infrastructure.Persistence;

public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
{
    private readonly QuizzerAppContext _context;
    public QuestionRepository(QuizzerAppContext context) : base(context)
    {
        _context = context;
    }
    public async Task CreateAsync(Question question) => Create(question);
    public void DeleteAsync(Question question) => Delete(question);
    public void UpdateAsync(Question question) => Update(question);
    public async Task<List<Question>> GetAllAsync() => FindAll(false).ToList();
    public async Task<Question> GetAsync(Guid id) => await FindByCondition(q => q.Id == new QuestionId(id), false).FirstOrDefaultAsync();
    public async Task<List<Question>> GetAllByUserIdAsync(string userId) => FindAll(false).OrderBy(q => q.UserId).ToList();
    public  int GetQuestionVoteCount(Guid id) => _context.QuestionVotes.Where(qv => qv.QuestionId == new QuestionId(id)).Count();

    public IQueryable<Question> GetQueriable() => Queriable();
}
