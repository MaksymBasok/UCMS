using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Assignments;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;
public sealed class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _db;
    public AssignmentRepository(ApplicationDbContext db) => _db = db;

    public Task<Assignment?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Assignments.FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task AddAsync(Assignment entity, CancellationToken ct)
    { _db.Assignments.Add(entity); return Task.CompletedTask; }

    public Task RemoveAsync(Assignment entity, CancellationToken ct)
    { _db.Assignments.Remove(entity); return Task.CompletedTask; }
}
