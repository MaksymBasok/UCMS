using MediatR;

namespace UCMS.Application.Features.Submissions.Commands.CompleteSubmission;
public sealed record CompleteSubmissionCommand(Guid Id, string Notes, decimal Grade) : IRequest<Unit>;
