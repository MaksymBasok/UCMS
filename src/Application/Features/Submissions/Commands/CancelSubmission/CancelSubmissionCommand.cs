using MediatR;

namespace UCMS.Application.Features.Submissions.Commands.CancelSubmission;

public sealed record CancelSubmissionCommand(Guid Id) : IRequest<MediatR.Unit>;
