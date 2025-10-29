using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Assignments;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _db;

    public AssignmentRepository(ApplicationDbContext db) => _db = db;

    public async Task<Assignment> AddAsync(Assignment entity, CancellationToken ct)
    {
        await _db.Assignments.AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);

        return entity;
    }

    public Task<Assignment?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Assignments.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<Assignment> UpdateAsync(Assignment entity, CancellationToken ct)
    {
        _db.Assignments.Update(entity);
        await _db.SaveChangesAsync(ct);

        return entity;
    }

    public async Task<Assignment> RemoveAsync(Assignment entity, CancellationToken ct)
    {
        _db.Assignments.Remove(entity);
        await _db.SaveChangesAsync(ct);

        return entity;
    }
}
