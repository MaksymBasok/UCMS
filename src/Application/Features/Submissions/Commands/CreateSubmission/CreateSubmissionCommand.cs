using LanguageExt;
using MediatR;
using UCMS.Application.Features.Submissions.Dtos;
using UCMS.Application.Features.Submissions.Exceptions;

namespace UCMS.Application.Features.Submissions.Commands.CreateSubmission;

public sealed record CreateSubmissionCommand(
    Guid AssignmentId,
    Guid StudentId,
    string ContentUrl,
    DateTime SubmittedAtUtc)
    : IRequest<Either<SubmissionException, SubmissionDto>>;
