using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.Courses.Commands.CreateCourse;
using UCMS.Application.Features.Courses.Commands.UpdateCourse;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Application.Features.Courses.Queries.GetCourseById;
using UCMS.Application.Features.Courses.Queries.GetCourses;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/courses")]
public sealed class CoursesController : ControllerBase
{
    private readonly IMediator _m; public CoursesController(IMediator m) => _m = m;

    [HttpPost]
    public async Task<ActionResult<CourseDto>> Create(CreateCourseCommand cmd, CancellationToken ct)
        => CreatedAtAction(nameof(GetById), new { id = (await _m.Send(cmd, ct)).Id }, await _m.Send(cmd, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetAll(CancellationToken ct)
        => Ok(await _m.Send(new GetCoursesQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseDto?>> GetById(Guid id, CancellationToken ct)
        => Ok(await _m.Send(new GetCourseByIdQuery(id), ct));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourseDto>> Update(Guid id, [FromBody] UpdateCourseCommand body, CancellationToken ct)
    => Ok(await _m.Send(body with { Id = id }, ct));
}