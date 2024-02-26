using Microsoft.EntityFrameworkCore;
using QuizzenApp.Domain.Entities.ExamAggregate;
using QuizzenApp.Domain.Entities.UserAggregate;
using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Infrastructure.Contracts;
using QuizzerApp.Infrastructure.EFCore.Contexts;

namespace QuizzerApp.Infrastructure.Persistence;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly QuizzerAppContext _context ;
    public UserRepository(QuizzerAppContext context) : base(context)
    {
        _context = context;    
    }

    

    public bool CheckIsExist(string userId) => FindByCondition(u => u.Id == userId, false) is not null;


    public IQueryable<User> GetQueriable() => Queriable();

    public async Task<Exam> GetUserExamAsync(Guid examId) => await _context.Exams.FirstOrDefaultAsync(e => e.Id == examId);

}
