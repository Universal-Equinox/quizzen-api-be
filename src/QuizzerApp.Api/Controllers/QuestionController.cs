using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizzerApp.Application.Features.Commands.Question;
using QuizzerApp.Application.Features.Commands.Question.CreateQuestionVote;
using QuizzerApp.Application.Features.Commands.Question.DeleteQuestionVote;
using QuizzerApp.Application.Features.Queries.Question.ReadQuestionById;
using QuizzerApp.Application.Features.Queries.Question.ReadQuestions;

namespace QuizzerApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly ISender _sender;

    public QuestionController(ISender sender)
    {
        _sender = sender;
    }


    [HttpPost]
    public async Task<ActionResult> CreateQuestion(CreateQuestionCommand command)
    {

        var res = await _sender.Send(command);

        if (res)
            return StatusCode(201);

        throw new Exception();


    }

    [HttpGet]
    public async Task<ActionResult> GetAllQuestions([FromQuery] string? exam, [FromQuery] string? subject, [FromQuery] string? topic, string? userId)
    {
        ReadQuestionQuery query = new(Exam: exam, Subject: subject, Topic: topic, UserId: userId);
        var res = await _sender.Send(query);

        return  Ok(res);

    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetQuestionById([FromRoute] Guid id)
    {
        ReadQuestionByIdQuery query = new(QuestionId: id);
        var res = await _sender.Send(query);

        return Ok(res);

    }

    [HttpPost("vote")]
    public async Task<IActionResult> QuestionUpVote([FromQuery] CreateQuestionVoteCommand command)
    {
        var res = await _sender.Send(command);
        return StatusCode(201);
    }

    [HttpDelete("vote")]
    public async Task<IActionResult> DeleteQuestionVote([FromQuery] DeleteQuestionVoteCommand command)
    {
        var res = await _sender.Send(command);
        return StatusCode(201);
    }



}
