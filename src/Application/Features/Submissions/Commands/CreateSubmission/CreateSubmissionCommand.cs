using MediatR;
using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Features.Submissions.Commands.CreateSubmission;
public sealed record CreateSubmissionCommand(Guid AssignmentId, Guid StudentId, string ContentUrl, DateTime SubmittedAtUtc) : IRequest<SubmissionDto>;
