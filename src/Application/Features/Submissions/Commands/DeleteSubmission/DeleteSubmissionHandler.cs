using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Exceptions;

namespace UCMS.Application.Features.Submissions.Commands.DeleteSubmission;

public sealed class DeleteSubmissionHandler : IRequestHandler<DeleteSubmissionCommand, Unit>
{
    private readonly ISubmissionRepository _repo;

    public DeleteSubmissionHandler(ISubmissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(DeleteSubmissionCommand request, CancellationToken ct)
    {
        try
        {
            var submission = await _repo.GetByIdAsync(request.Id, ct)
                ?? throw new SubmissionNotFoundException(request.Id);

            await _repo.RemoveAsync(submission, ct);

            return Unit.Value;
        }
        catch (SubmissionException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new SubmissionUnexpectedException(request.Id, ex);
        }
    }
}
