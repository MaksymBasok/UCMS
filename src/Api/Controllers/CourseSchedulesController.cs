using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Commands.DeactivateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Application.Features.CourseSchedules.Queries.GetAllSchedules;
using UCMS.Application.Features.CourseSchedules.Queries.GetByCourse;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/course-schedules")]
public sealed class CourseSchedulesController : ControllerBase
{
    private readonly IMediator _m; public CourseSchedulesController(IMediator m) => _m = m;

    [HttpPost]
    public async Task<ActionResult<CourseScheduleDto>> Create(CreateCourseScheduleCommand cmd, CancellationToken ct)
        => Ok(await _m.Send(cmd, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseScheduleDto>>> GetAll(CancellationToken ct)
        => Ok(await _m.Send(new GetAllSchedulesQuery(), ct));

    [HttpGet("course/{courseId:guid}")]
    public async Task<ActionResult<IReadOnlyList<CourseScheduleDto>>> ByCourse(Guid courseId, CancellationToken ct)
        => Ok(await _m.Send(new GetSchedulesByCourseQuery(courseId), ct));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourseScheduleDto>> Update(Guid id, [FromBody] UpdateCourseScheduleCommand body, CancellationToken ct)
        => Ok(await _m.Send(body with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    { await _m.Send(new DeactivateCourseScheduleCommand(id), ct); return NoContent(); }
}
