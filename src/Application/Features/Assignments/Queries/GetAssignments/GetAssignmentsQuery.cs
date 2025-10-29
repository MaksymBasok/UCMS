namespace UCMS.Application.Features.Assignments.Queries.GetAssignments;
using MediatR;
using UCMS.Application.Features.Assignments.Dtos;

public sealed record GetAssignmentsQuery() : IRequest<IReadOnlyList<AssignmentDto>>;
