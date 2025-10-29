using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Exceptions;

namespace UCMS.Application.Features.Submissions.Commands.CompleteSubmission;

public sealed class CompleteSubmissionHandler : IRequestHandler<CompleteSubmissionCommand, MediatR.Unit>
{
    private readonly ISubmissionRepository _repo;

    public CompleteSubmissionHandler(ISubmissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<MediatR.Unit> Handle(CompleteSubmissionCommand request, CancellationToken ct)
    {
        try
        {
            var submission = await _repo.GetByIdAsync(request.Id, ct)
                ?? throw new SubmissionNotFoundException(request.Id);

            submission.Complete(request.Notes, request.Grade);
            await _repo.UpdateAsync(submission, ct);

            return MediatR.Unit.Value;
        }
        catch (ArgumentException ex)
        {
            throw new SubmissionValidationException(request.Id, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new SubmissionValidationException(request.Id, ex.Message);
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
