using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Api.Modules.Errors;
using UCMS.Application.Features.Enrollments.Commands.CompleteEnrollment;
using UCMS.Application.Features.Enrollments.Commands.CreateEnrollment;
using UCMS.Application.Features.Enrollments.Commands.DropEnrollment;
using UCMS.Application.Features.Enrollments.Commands.DeleteEnrollment;
using UCMS.Application.Features.Enrollments.Dtos;
using UCMS.Application.Features.Enrollments.Exceptions;
using UCMS.Application.Features.Enrollments.Queries.GetEnrollmentById;
using UCMS.Application.Features.Enrollments.Queries.GetEnrollments;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/enrollments")]
public sealed class EnrollmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnrollmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<EnrollmentDto>> Enroll(
        [FromBody] CreateEnrollmentCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.Match<ActionResult<EnrollmentDto>>(
            created => CreatedAtAction(nameof(GetById), new { id = created.Id }, created),
            error => error.ToObjectResult());
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EnrollmentDto>>> GetAll(
        [FromQuery] Guid? studentId,
        [FromQuery] Guid? courseId,
        CancellationToken ct)
    {
        var response = await _mediator.Send(new GetEnrollmentsQuery(studentId, courseId), ct);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EnrollmentDto?>> GetById(Guid id, CancellationToken ct)
    {
        var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(id), ct);
        return enrollment is null ? NotFound() : Ok(enrollment);
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<ActionResult<EnrollmentDto>> Complete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new CompleteEnrollmentCommand(id), ct);
        return result.Match<ActionResult<EnrollmentDto>>(value => Ok(value), error => error.ToObjectResult());
    }

    [HttpPatch("{id:guid}/drop")]
    public async Task<ActionResult<EnrollmentDto>> Drop(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DropEnrollmentCommand(id), ct);
        return result.Match<ActionResult<EnrollmentDto>>(value => Ok(value), error => error.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteEnrollmentCommand(id), ct);

        return result.Match<IActionResult>(
            _ => NoContent(),
            error => error.ToObjectResult());
    }
}
