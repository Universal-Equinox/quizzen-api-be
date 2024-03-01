using QuizzerApp.Application.Common.Interfaces;
using QuizzerApp.Infrastructure.Dapper.Contexts;
using QuizzerApp.Infrastructure.EFCore.Contexts;

namespace QuizzerApp.Infrastructure.Persistence;

public class RepositoryManager : IRepositoryManager
{
    private readonly QuizzerAppContext _context;
    private readonly QuizzerAppQueryContext _queryContext;

    private readonly Lazy<IQuestionRepository> _questionRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IAnswerRepository> _answerRepository;
    private readonly Lazy<IPhotoRepository> _photoRepository;

    private readonly Lazy<IExamRepository> _examRepository;

    public RepositoryManager(QuizzerAppContext context, QuizzerAppQueryContext queryContext)
    {
        _context = context;
        _queryContext = queryContext;

        _questionRepository = new Lazy<IQuestionRepository>(() => new QuestionRepository(context: _context, queryContext: _queryContext));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context: _context));
        _answerRepository = new Lazy<IAnswerRepository>(() => new AnswerRepository(context: _context));
        _photoRepository = new Lazy<IPhotoRepository>(() => new PhotoRepository(context: _context));
        _examRepository = new Lazy<IExamRepository>(() => new ExamRepository(context: _context));
    }

    public IQuestionRepository Question => _questionRepository.Value;
    public IUserRepository User => _userRepository.Value;
    public IAnswerRepository Answer => _answerRepository.Value;
    public IPhotoRepository Photo => _photoRepository.Value;

    public IExamRepository Exam => _examRepository.Value;

    public async Task<bool> SaveAsync() => await _context.SaveChangesAsync() > 0;

}
