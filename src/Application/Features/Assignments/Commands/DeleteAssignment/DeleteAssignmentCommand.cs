using MediatR;

namespace UCMS.Application.Features.Assignments.Commands.DeleteAssignment;

public sealed record DeleteAssignmentCommand(Guid Id) : IRequest<Unit>;
