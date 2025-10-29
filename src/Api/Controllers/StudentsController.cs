using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Api.Modules.Errors;
using UCMS.Application.Features.Students.Commands.CreateStudent;
using UCMS.Application.Features.Students.Commands.DeleteStudent;
using UCMS.Application.Features.Students.Commands.UpdateStudent;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Application.Features.Students.Queries.GetStudentById;
using UCMS.Application.Features.Students.Queries.GetStudents;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/students")]
public sealed class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(
        [FromBody] CreateStudentCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.Match<ActionResult<StudentDto>>(
            created => CreatedAtAction(nameof(GetById), new { id = created.Id }, created),
            error => error.ToObjectResult());
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetStudentsQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StudentDto>> GetById(Guid id, CancellationToken ct)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(id), ct);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StudentDto>> Update(
        Guid id,
        [FromBody] UpdateStudentCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command with { Id = id }, ct);

        return result.Match<ActionResult<StudentDto>>(
            updated => Ok(updated),
            error => error.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteStudentCommand(id), ct);

        return result.Match<IActionResult>(
            _ => NoContent(),
            error => error.ToObjectResult());
    }
}
