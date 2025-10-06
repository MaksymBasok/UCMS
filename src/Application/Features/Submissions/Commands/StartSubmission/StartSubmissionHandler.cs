using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;

namespace UCMS.Application.Features.Submissions.Commands.StartSubmission;
public sealed class StartSubmissionHandler : IRequestHandler<StartSubmissionCommand, Unit>
{
    private readonly ISubmissionRepository _repo; private readonly IUnitOfWork _uow;
    public StartSubmissionHandler(ISubmissionRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
    public async Task<Unit> Handle(StartSubmissionCommand r, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Submission not found");
        s.StartReview(); await _uow.SaveChangesAsync(ct); return Unit.Value;
    }
}
