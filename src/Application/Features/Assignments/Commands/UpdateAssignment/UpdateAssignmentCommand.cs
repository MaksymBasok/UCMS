namespace UCMS.Application.Features.Assignments.Commands.UpdateAssignment;
using MediatR;
using UCMS.Application.Features.Assignments.Dtos;

public sealed record UpdateAssignmentCommand(
    Guid Id, string Title, string Description, DateTime DueDate
) : IRequest<AssignmentDto>;
