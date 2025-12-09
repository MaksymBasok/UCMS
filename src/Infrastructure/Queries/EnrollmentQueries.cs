using System.Linq;
using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Enrollments.Dtos;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Queries;

public sealed class EnrollmentQueries : IEnrollmentQueries
{
    private readonly ApplicationDbContext _db;

    public EnrollmentQueries(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<EnrollmentDto>> GetAllAsync(CancellationToken ct)
        => await _db.Enrollments
            .AsNoTracking()
            .Select(EnrollmentDto.From)
            .ToListAsync(ct);

    public Task<EnrollmentDto?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Enrollments
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(EnrollmentDto.From)
            .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<EnrollmentDto>> GetByStudentAsync(Guid studentId, CancellationToken ct)
        => await _db.Enrollments
            .AsNoTracking()
            .Where(x => x.StudentId == studentId)
            .Select(EnrollmentDto.From)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<EnrollmentDto>> GetByCourseAsync(Guid courseId, CancellationToken ct)
        => await _db.Enrollments
            .AsNoTracking()
            .Where(x => x.CourseId == courseId)
            .Select(EnrollmentDto.From)
            .ToListAsync(ct);

    public Task<EnrollmentDto?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct)
        => _db.Enrollments
            .AsNoTracking()
            .Where(x => x.StudentId == studentId && x.CourseId == courseId)
            .Select(EnrollmentDto.From)
            .FirstOrDefaultAsync(ct);
}
