using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.Submissions.Commands.CancelSubmission;
using UCMS.Application.Features.Submissions.Commands.CompleteSubmission;
using UCMS.Application.Features.Submissions.Commands.CreateSubmission;
using UCMS.Application.Features.Submissions.Commands.StartSubmission;
using UCMS.Application.Features.Submissions.Commands.UpdateSubmission;
using UCMS.Application.Features.Submissions.Dtos;
using UCMS.Application.Features.Submissions.Queries.GetSubmissionById;
using UCMS.Application.Features.Submissions.Queries.GetSubmissions;

namespace UCMS.Api.Controllers;

[ApiController]
[Route("api/submissions")]
public sealed class SubmissionsController : ControllerBase
{
    private readonly IMediator _m; public SubmissionsController(IMediator m) => _m = m;

    [HttpPost]
    public async Task<ActionResult<SubmissionDto>> Create(CreateSubmissionCommand cmd, CancellationToken ct)
    {
        var created = await _m.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SubmissionDto>>> GetAll(CancellationToken ct)
        => Ok(await _m.Send(new GetSubmissionsQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SubmissionDto?>> GetById(Guid id, CancellationToken ct)
        => Ok(await _m.Send(new GetSubmissionByIdQuery(id), ct));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SubmissionDto>> Update(Guid id, UpdateSubmissionCommand body, CancellationToken ct)
        => Ok(await _m.Send(body with { Id = id }, ct));

    [HttpPatch("{id:guid}/start")]
    public async Task<IActionResult> Start(Guid id, CancellationToken ct)
    { await _m.Send(new StartSubmissionCommand(id), ct); return NoContent(); }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteSubmissionCommand body, CancellationToken ct)
    { await _m.Send(body with { Id = id }, ct); return NoContent(); }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    { await _m.Send(new CancelSubmissionCommand(id), ct); return NoContent(); }
}
