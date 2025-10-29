using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;

namespace UCMS.Application.Features.Submissions.Commands.StartSubmission;

public sealed class StartSubmissionHandler : IRequestHandler<StartSubmissionCommand, Unit>
{
    private readonly ISubmissionRepository _repo;

    public StartSubmissionHandler(ISubmissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(StartSubmissionCommand request, CancellationToken ct)
    {
        var submission = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Submission not found");

        submission.StartReview();
        await _repo.UpdateAsync(submission, ct);

        return Unit.Default;
    }
}
