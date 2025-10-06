using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Queries;

public sealed class StudentQueries : IStudentQueries
{
    private readonly ApplicationDbContext _db;
    public StudentQueries(ApplicationDbContext db) => _db = db;

    public Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken ct) =>
        _db.Students
           .AsNoTracking()
           .Select(s => new StudentDto(s.Id, s.StudentNumber, s.FullName, s.Email, s.GroupId))
           .ToListAsync(ct)
           .ContinueWith(t => (IReadOnlyList<StudentDto>)t.Result, ct);

    public Task<StudentDto?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Students
           .AsNoTracking()
           .Where(s => s.Id == id)
           .Select(s => new StudentDto(s.Id, s.StudentNumber, s.FullName, s.Email, s.GroupId))
           .FirstOrDefaultAsync(ct);
}
