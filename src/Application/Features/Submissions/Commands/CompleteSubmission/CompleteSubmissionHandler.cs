using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;

namespace UCMS.Application.Features.Submissions.Commands.CompleteSubmission;
public sealed class CompleteSubmissionHandler : IRequestHandler<CompleteSubmissionCommand, Unit>
{
    private readonly ISubmissionRepository _repo; private readonly IUnitOfWork _uow;
    public CompleteSubmissionHandler(ISubmissionRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
    public async Task<Unit> Handle(CompleteSubmissionCommand r, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Submission not found");
        s.Complete(r.Notes, r.Grade); await _uow.SaveChangesAsync(ct); return Unit.Value;
    }
}
