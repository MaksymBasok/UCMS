using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Dtos;
using UCMS.Domain.Submissions;

namespace UCMS.Application.Features.Submissions.Commands.CreateSubmission;
public sealed class CreateSubmissionHandler : IRequestHandler<CreateSubmissionCommand, SubmissionDto>
{
    private readonly ISubmissionRepository _repo; private readonly IUnitOfWork _uow;
    public CreateSubmissionHandler(ISubmissionRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
    public async Task<SubmissionDto> Handle(CreateSubmissionCommand r, CancellationToken ct)
    {
        var s = Submission.New(Guid.NewGuid(), r.AssignmentId, r.StudentId, r.ContentUrl, r.SubmittedAtUtc);
        await _repo.AddAsync(s, ct); await _uow.SaveChangesAsync(ct); return SubmissionDto.From(s);
    }
}
