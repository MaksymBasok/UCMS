using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Dtos;
using UCMS.Application.Features.Submissions.Exceptions;

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
        try
        {
            var submission = await _repo.GetByIdAsync(request.Id, ct)
                ?? throw new SubmissionNotFoundException(request.Id);

            submission.UpdateContent(request.ContentUrl);
            await _repo.UpdateAsync(submission, ct);

            return SubmissionDto.From(submission);
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
