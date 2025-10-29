using UCMS.Domain.Assignments;

namespace UCMS.Application.Abstractions.Repositories;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Assignment> AddAsync(Assignment entity, CancellationToken ct);
    Task<Assignment> UpdateAsync(Assignment entity, CancellationToken ct);
    Task<Assignment> RemoveAsync(Assignment entity, CancellationToken ct);
}
