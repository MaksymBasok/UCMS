namespace UCMS.Application.Features.Assignments.Commands.CreateAssignment;
using MediatR;
using UCMS.Application.Features.Assignments.Dtos;


public sealed record CreateAssignmentCommand(
    Guid CourseId, string Title, string Description, DateTime DueDate
) : IRequest<AssignmentDto>;
