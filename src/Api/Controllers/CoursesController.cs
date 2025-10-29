using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Api.Modules.Errors;
using UCMS.Application.Features.Courses.Commands.CreateCourse;
using UCMS.Application.Features.Courses.Commands.DeleteCourse;
using UCMS.Application.Features.Courses.Commands.UpdateCourse;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Application.Features.Courses.Queries.GetCourseById;
using UCMS.Application.Features.Courses.Queries.GetCourses;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/courses")]
public sealed class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CourseDto>> Create(
        [FromBody] CreateCourseCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.Match<ActionResult<CourseDto>>(
            created => CreatedAtAction(nameof(GetById), new { id = created.Id }, created),
            error => error.ToObjectResult());
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetCoursesQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseDto>> GetById(Guid id, CancellationToken ct)
    {
        var course = await _mediator.Send(new GetCourseByIdQuery(id), ct);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourseDto>> Update(
        Guid id,
        [FromBody] UpdateCourseCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command with { Id = id }, ct);

        return result.Match<ActionResult<CourseDto>>(
            updated => Ok(updated),
            error => error.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteCourseCommand(id), ct);

        return result.Match<IActionResult>(
            _ => NoContent(),
            error => error.ToObjectResult());
    }
}
