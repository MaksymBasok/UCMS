using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.Students.Commands.CreateStudent;
using UCMS.Application.Features.Students.Queries.GetStudents;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/students")]
public sealed class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public StudentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentCommand cmd, CancellationToken ct)
    {
        var dto = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var list = await _mediator.Send(new GetStudentsQuery(), ct);
        return Ok(list);
    }
}
