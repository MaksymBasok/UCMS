using MediatR;

namespace UCMS.Application.Features.Submissions.Commands.StartSubmission;
public sealed record StartSubmissionCommand(Guid Id) : IRequest<Unit>;
