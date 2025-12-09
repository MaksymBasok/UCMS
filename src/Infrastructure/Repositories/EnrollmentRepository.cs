using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Enrollments;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ApplicationDbContext _db;

    public EnrollmentRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken ct)
    {
        await _db.Enrollments.AddAsync(enrollment, ct);
        await _db.SaveChangesAsync(ct);

        return enrollment;
    }

    public Task<Enrollment?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Enrollments.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct)
        => _db.Enrollments.FirstOrDefaultAsync(x => x.StudentId == studentId && x.CourseId == courseId, ct);

    public async Task<Enrollment> UpdateAsync(Enrollment enrollment, CancellationToken ct)
    {
        _db.Enrollments.Update(enrollment);
        await _db.SaveChangesAsync(ct);

        return enrollment;
    }
}
