namespace QuizzerApp.Application.Dtos.Answer;

public class AnswerReponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string Status { get; set; }
    public Guid QuestionId { get; set; }
    public UserDto User { get; set; }
    public List<string> Images { get; set; } = new();
    public DateTime CreatedDate { get; set; }



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
