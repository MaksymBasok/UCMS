namespace UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Assignments.Dtos;

public interface IAssignmentQueries
{
    Task<IReadOnlyList<AssignmentDto>> GetAllAsync(CancellationToken ct);
    Task<AssignmentDto?> GetByIdAsync(Guid id, CancellationToken ct);
}
