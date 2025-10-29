using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;

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
        var submission = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Submission not found");

        submission.Cancel();
        await _repo.UpdateAsync(submission, ct);

        return Unit.Default;
    }
}
