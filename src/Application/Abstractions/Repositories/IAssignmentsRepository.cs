namespace UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Assignments;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Assignment entity, CancellationToken ct);
    Task RemoveAsync(Assignment entity, CancellationToken ct);
}
