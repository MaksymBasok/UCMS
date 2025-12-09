using MediatR;

namespace UCMS.Application.Features.Submissions.Commands.DeleteSubmission;

public sealed record DeleteSubmissionCommand(Guid Id) : IRequest<Unit>;
