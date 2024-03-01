namespace QuizzerApp.Application.Dtos.Question;

public class QuestionResponse
{

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserDto User { get; set; }
    public TagsDto Tags { get; set; }
    // public List<ImageDto> Images { get; set; } 
    public List<string> Images { get; set; } = new();
    public int AnswerCount { get; set; }
    public int VoteCount { get; set; }
}

public class TagsDto
{
    public string Exam { get; set; }
    public string Subject { get; set; }
    public string Topic { get; set; }
}

public class UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfileImg { get; set; }
}

public record ImageDto
{
    public Guid QId { get; set; }
    public string Url { get; set; }
}
