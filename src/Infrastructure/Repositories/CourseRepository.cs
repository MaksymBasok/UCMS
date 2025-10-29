using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Courses;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _db;

    public CourseRepository(ApplicationDbContext db) => _db = db;

    public async Task<Course> AddAsync(Course course, CancellationToken ct)
    {
        await _db.Courses.AddAsync(course, ct);
        await _db.SaveChangesAsync(ct);

        return course;
    }

    public Task<Course?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Courses.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Course?> GetByCodeAsync(string code, CancellationToken ct)
        => _db.Courses.FirstOrDefaultAsync(x => x.Code == code, ct);

    public async Task<bool> IsCodeUniqueAsync(string code, CancellationToken ct)
    {
        return !await _db.Courses.AnyAsync(x => x.Code == code, ct);
    }

    public async Task<Course> UpdateAsync(Course course, CancellationToken ct)
    {
        _db.Courses.Update(course);
        await _db.SaveChangesAsync(ct);

        return course;
    }

    public async Task<Course> RemoveAsync(Course course, CancellationToken ct)
    {
        _db.Courses.Remove(course);
        await _db.SaveChangesAsync(ct);

        return course;
    }
}
