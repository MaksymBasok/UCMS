namespace UCMS.Application.Features.Assignments.Queries.GetAssignmentById;
using MediatR;
using UCMS.Application.Features.Assignments.Dtos;

public sealed record GetAssignmentByIdQuery(Guid Id) : IRequest<AssignmentDto?>;
