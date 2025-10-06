using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Features.Submissions.Commands.UpdateSubmission;
public sealed class UpdateSubmissionHandler : IRequestHandler<UpdateSubmissionCommand, SubmissionDto>
{
    private readonly ISubmissionRepository _repo; private readonly IUnitOfWork _uow;
    public UpdateSubmissionHandler(ISubmissionRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
    public async Task<SubmissionDto> Handle(UpdateSubmissionCommand r, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Submission not found");
        s.UpdateContent(r.ContentUrl); await _uow.SaveChangesAsync(ct); return SubmissionDto.From(s);
    }
}
