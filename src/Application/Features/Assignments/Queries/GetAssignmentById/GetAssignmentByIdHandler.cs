namespace UCMS.Application.Features.Assignments.Queries.GetAssignmentById;
using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Assignments.Dtos;

public sealed class GetAssignmentByIdHandler : IRequestHandler<GetAssignmentByIdQuery, AssignmentDto?>
{
    private readonly IAssignmentQueries _q;
    public GetAssignmentByIdHandler(IAssignmentQueries q) => _q = q;

    public Task<AssignmentDto?> Handle(GetAssignmentByIdQuery r, CancellationToken ct)
        => _q.GetByIdAsync(r.Id, ct);
}
