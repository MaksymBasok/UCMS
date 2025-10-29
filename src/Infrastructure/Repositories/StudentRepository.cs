using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Students;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _db;

    public StudentRepository(ApplicationDbContext db) => _db = db;

    public async Task<Student> AddAsync(Student student, CancellationToken ct)
    {
        await _db.Students.AddAsync(student, ct);
        await _db.SaveChangesAsync(ct);

        return student;
    }

    public Task<Student?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Students.FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<Student?> GetByStudentNumberAsync(string studentNumber, CancellationToken ct)
        => _db.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber, ct);

    public async Task<bool> IsStudentNumberUniqueAsync(string studentNumber, CancellationToken ct)
    {
        return !await _db.Students.AnyAsync(s => s.StudentNumber == studentNumber, ct);
    }

    public async Task<Student> UpdateAsync(Student student, CancellationToken ct)
    {
        _db.Students.Update(student);
        await _db.SaveChangesAsync(ct);

        return student;
    }

    public async Task<Student> RemoveAsync(Student student, CancellationToken ct)
    {
        _db.Students.Remove(student);
        await _db.SaveChangesAsync(ct);

        return student;
    }
}
