using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Exceptions;

namespace UCMS.Application.Features.Submissions.Commands.CompleteSubmission;

public sealed class CompleteSubmissionHandler : IRequestHandler<CompleteSubmissionCommand, Unit>
{
    private readonly ISubmissionRepository _repo;

    public CompleteSubmissionHandler(ISubmissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(CompleteSubmissionCommand request, CancellationToken ct)
    {
        var submission = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new SubmissionNotFoundException(request.Id);

        submission.Complete(request.Notes, request.Grade);
        await _repo.UpdateAsync(submission, ct);

        return Unit.Value;
    }
}
