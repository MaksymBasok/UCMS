using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Features.Submissions.Commands.UpdateSubmission;

public sealed class UpdateSubmissionHandler : IRequestHandler<UpdateSubmissionCommand, SubmissionDto>
{
    private readonly ISubmissionRepository _repo;

    public UpdateSubmissionHandler(ISubmissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<SubmissionDto> Handle(UpdateSubmissionCommand request, CancellationToken ct)
    {
        var submission = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Submission not found");

        submission.UpdateContent(request.ContentUrl);
        await _repo.UpdateAsync(submission, ct);

        return SubmissionDto.From(submission);
    }
}
