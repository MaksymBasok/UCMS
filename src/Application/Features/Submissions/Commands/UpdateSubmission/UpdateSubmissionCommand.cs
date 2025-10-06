using MediatR;
using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Features.Submissions.Commands.UpdateSubmission;
public sealed record UpdateSubmissionCommand(Guid Id, string ContentUrl) : IRequest<SubmissionDto>;
