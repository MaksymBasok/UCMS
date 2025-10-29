using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Exceptions;

namespace UCMS.Application.Features.Submissions.Commands.CancelSubmission;

public sealed class CancelSubmissionHandler : IRequestHandler<CancelSubmissionCommand, Unit>
{
    private readonly ISubmissionRepository _repo;

    public CancelSubmissionHandler(ISubmissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(CancelSubmissionCommand request, CancellationToken ct)
    {
        var submission = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new SubmissionNotFoundException(request.Id);

        submission.Cancel();
        await _repo.UpdateAsync(submission, ct);

        return Unit.Value;
    }
}
