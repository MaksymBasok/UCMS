using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Students;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _db;

    public StudentRepository(ApplicationDbContext db) => _db = db;

    public Task<Student?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Students.FindAsync([id], ct).AsTask();

    public Task<Student?> GetByStudentNumberAsync(string studentNumber, CancellationToken ct)
        => _db.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber, ct);

    public Task<Student?> GetByStudentNumberAsync(string studentNumber, CancellationToken ct)
        => _db.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber, ct);

    public Task AddAsync(Student student, CancellationToken ct)
    {
        _db.Students.Add(student);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Student student, CancellationToken ct)
    {
        _db.Students.Remove(student);
        return Task.CompletedTask;
    }
}
