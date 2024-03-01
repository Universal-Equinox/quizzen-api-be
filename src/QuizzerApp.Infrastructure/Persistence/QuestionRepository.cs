using Dapper;
using QuizzenApp.Domain.Entities.QuestionAggregate;
using QuizzenApp.Domain.Entities.QuestionAggregate.ValueObjects;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Infrastructure.Contracts;
using QuizzerApp.Infrastructure.Dapper.Contexts;
using QuizzerApp.Infrastructure.EFCore.Contexts;

namespace QuizzerApp.Infrastructure.Persistence;

public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
{
    private readonly QuizzerAppContext _context;
    private readonly QuizzerAppQueryContext _queryContext;
    public QuestionRepository(QuizzerAppContext context, QuizzerAppQueryContext queryContext) : base(context)
    {
        _context = context;
        _queryContext = queryContext;
    }
    public async Task CreateAsync(Question question) => Create(question);
    public void DeleteAsync(Question question) => Delete(question);
    public void UpdateAsync(Question question) => Update(question);
    public async Task<List<Question>> GetAllAsync()
    {
        string query = "SELECT * FROM Questions";
        using var conn = _queryContext.Connect();

        var res = await conn.QueryAsync<Question>(query);

        return res.ToList();


    }
    public async Task<Question> GetAsync(Guid id)
    {
        string query = "SELECT * FROM Questions WHERE Id = @categoryId";

        using var conn = _queryContext.Connect();


        var queryParams = new DynamicParameters();
        queryParams.Add("@categoryId", id);

        var res = await conn.QueryFirstOrDefaultAsync<Question>(query);

        return res;
    }
    public async Task<List<Question>> GetAllByUserIdAsync(string userId) => FindAll(false).OrderBy(q => q.UserId).ToList();
    public int GetQuestionVoteCount(Guid id) => _context.QuestionVotes.Where(qv => qv.QuestionId == new QuestionId(id)).Count();

    public IQueryable<Question> GetQueriable() => Queriable();
}
