namespace UCMS.Application.Features.Assignments.Commands.PublishAssignment;
using MediatR;
using UCMS.Application.Features.Assignments.Dtos;

public sealed record PublishAssignmentCommand(Guid Id) : IRequest<AssignmentDto>;
