namespace UCMS.Application.Features.Assignments.Commands.CloseAssignment;
using MediatR;
using UCMS.Application.Features.Assignments.Dtos;

public sealed record CloseAssignmentCommand(Guid Id) : IRequest<AssignmentDto>;
