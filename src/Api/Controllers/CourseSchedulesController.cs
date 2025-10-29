using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Api.Modules.Errors;
using UCMS.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Commands.DeactivateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Application.Features.CourseSchedules.Exceptions;
using UCMS.Application.Features.CourseSchedules.Queries.GetAllSchedules;
using UCMS.Application.Features.CourseSchedules.Queries.GetByCourse;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/course-schedules")]
public sealed class CourseSchedulesController : ControllerBase
{
    private readonly IMediator _m;

    public CourseSchedulesController(IMediator m) => _m = m;

    [HttpPost]
    public async Task<ActionResult<CourseScheduleDto>> Create(CreateCourseScheduleCommand cmd, CancellationToken ct)
    {
        var result = await _m.Send(cmd, ct);

        return result.Match<ActionResult<CourseScheduleDto>>(
            created => Created($"api/course-schedules/{created.Id}", created),
            error => error.ToObjectResult());
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseScheduleDto>>> GetAll(CancellationToken ct)
        => Ok(await _m.Send(new GetAllSchedulesQuery(), ct));

    [HttpGet("course/{courseId:guid}")]
    public async Task<ActionResult<IReadOnlyList<CourseScheduleDto>>> ByCourse(Guid courseId, CancellationToken ct)
        => Ok(await _m.Send(new GetSchedulesByCourseQuery(courseId), ct));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourseScheduleDto>> Update(
        Guid id,
        [FromBody] UpdateCourseScheduleCommand body,
        CancellationToken ct)
    {
        try
        {
            var updated = await _m.Send(body with { Id = id }, ct);
            return Ok(updated);
        }
        catch (CourseScheduleException error)
        {
            return error.ToObjectResult();
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        try
        {
            await _m.Send(new DeactivateCourseScheduleCommand(id), ct);
            return NoContent();
        }
        catch (CourseScheduleException error)
        {
            return error.ToObjectResult();
        }
    }
}
