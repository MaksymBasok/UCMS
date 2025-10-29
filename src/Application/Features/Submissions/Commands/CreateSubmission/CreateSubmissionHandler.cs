using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Submissions.Dtos;
using UCMS.Application.Features.Submissions.Exceptions;
using UCMS.Domain.Submissions;

namespace UCMS.Application.Features.Submissions.Commands.CreateSubmission;

public sealed class CreateSubmissionHandler
    : IRequestHandler<CreateSubmissionCommand, Either<SubmissionException, SubmissionDto>>
{
    private readonly ISubmissionRepository _repo;
    private readonly ISubmissionQueries _queries;

    public CreateSubmissionHandler(
        ISubmissionRepository repo,
        ISubmissionQueries queries)
    {
        _repo = repo;
        _queries = queries;
    }

    public async Task<Either<SubmissionException, SubmissionDto>> Handle(
        CreateSubmissionCommand request,
        CancellationToken ct)
    {
        try
        {
            var existing = await _queries.GetByAssignmentAndStudentAsync(request.AssignmentId, request.StudentId, ct);
            if (existing is not null)
            {
                return new SubmissionAlreadyExistsException(existing.Id);
            }

            var submission = Submission.New(
                Guid.NewGuid(),
                request.AssignmentId,
                request.StudentId,
                request.ContentUrl);

            await _repo.AddAsync(submission, ct);

            return SubmissionDto.From(submission);
        }
        catch (ArgumentException ex)
        {
            return new SubmissionValidationException(Guid.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            return new SubmissionUnexpectedException(Guid.Empty, ex);
        }
    }
}
