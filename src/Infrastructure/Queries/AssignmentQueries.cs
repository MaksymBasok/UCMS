using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Assignments.Dtos;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Queries;

public sealed class AssignmentQueries : IAssignmentQueries
{
    private readonly ApplicationDbContext _db;
    public AssignmentQueries(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<AssignmentDto>> GetAllAsync(CancellationToken ct)
        => await _db.Assignments.AsNoTracking()
           .OrderBy(a => a.DueDate)
           .Select(a => new AssignmentDto
           {
               Id = a.Id,
               CourseId = a.CourseId,
               Title = a.Title,
               Description = a.Description,
               DueDate = a.DueDate,
               Status = a.Status.ToString(),
               CreatedAt = a.CreatedAt,
               UpdatedAt = a.UpdatedAt
           }).ToListAsync(ct);

    public async Task<AssignmentDto?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _db.Assignments.AsNoTracking()
           .Where(a => a.Id == id)
           .Select(a => new AssignmentDto
           {
               Id = a.Id,
               CourseId = a.CourseId,
               Title = a.Title,
               Description = a.Description,
               DueDate = a.DueDate,
               Status = a.Status.ToString(),
               CreatedAt = a.CreatedAt,
               UpdatedAt = a.UpdatedAt
           }).FirstOrDefaultAsync(ct);
}
