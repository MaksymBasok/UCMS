using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.Assignments.Dtos;
using UCMS.Application.Features.Assignments.Commands.CreateAssignment;
using UCMS.Application.Features.Assignments.Commands.UpdateAssignment;
using UCMS.Application.Features.Assignments.Commands.PublishAssignment;
using UCMS.Application.Features.Assignments.Commands.CloseAssignment;
using UCMS.Application.Features.Assignments.Commands.DeleteAssignment;
using UCMS.Application.Features.Assignments.Queries.GetAssignments;
using UCMS.Application.Features.Assignments.Queries.GetAssignmentById;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AssignmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssignmentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public Task<IReadOnlyList<AssignmentDto>> GetAll(CancellationToken ct)
        => _mediator.Send(new GetAssignmentsQuery(), ct);

    [HttpGet("{id:guid}")]
    public Task<AssignmentDto?> GetById(Guid id, CancellationToken ct)
        => _mediator.Send(new GetAssignmentByIdQuery(id), ct);

    [HttpPost]
    public Task<AssignmentDto> Create([FromBody] CreateAssignmentCommand cmd, CancellationToken ct)
        => _mediator.Send(cmd, ct);

    [HttpPut("{id:guid}")]
    public Task<AssignmentDto> Update(Guid id, [FromBody] UpdateAssignmentCommand body, CancellationToken ct)
        => _mediator.Send(body with { Id = id }, ct);

    [HttpPatch("{id:guid}/publish")]
    public Task<AssignmentDto> Publish(Guid id, CancellationToken ct)
        => _mediator.Send(new PublishAssignmentCommand(id), ct);

    [HttpPatch("{id:guid}/close")]
    public Task<AssignmentDto> Close(Guid id, CancellationToken ct)
        => _mediator.Send(new CloseAssignmentCommand(id), ct);

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new DeleteAssignmentCommand(id), ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
