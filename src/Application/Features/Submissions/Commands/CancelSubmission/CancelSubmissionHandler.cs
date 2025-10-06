using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;

namespace UCMS.Application.Features.Submissions.Commands.CancelSubmission;
public sealed class CancelSubmissionHandler : IRequestHandler<CancelSubmissionCommand, Unit>
{
    private readonly ISubmissionRepository _repo; private readonly IUnitOfWork _uow;
    public CancelSubmissionHandler(ISubmissionRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
    public async Task<Unit> Handle(CancelSubmissionCommand r, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Submission not found");
        s.Cancel(); await _uow.SaveChangesAsync(ct); return Unit.Value;
    }
}
