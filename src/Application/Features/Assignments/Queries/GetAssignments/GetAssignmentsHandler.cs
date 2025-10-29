namespace UCMS.Application.Features.Assignments.Queries.GetAssignments;
using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Assignments.Dtos;

public sealed class GetAssignmentsHandler : IRequestHandler<GetAssignmentsQuery, IReadOnlyList<AssignmentDto>>
{
    private readonly IAssignmentQueries _q;
    public GetAssignmentsHandler(IAssignmentQueries q) => _q = q;

    public Task<IReadOnlyList<AssignmentDto>> Handle(GetAssignmentsQuery r, CancellationToken ct)
        => _q.GetAllAsync(ct);
}
