using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizzerApp.Application.Features.Queries.Exam.GetExams;

namespace QuizzerApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamController : ControllerBase
{
    private readonly ISender _sender;

    public ExamController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet()]
    public async Task<IActionResult> GetExams([FromQuery] GetExamsQuery query)
    {
        var res = await _sender.Send(query);

        return Ok(res);
    }


}
