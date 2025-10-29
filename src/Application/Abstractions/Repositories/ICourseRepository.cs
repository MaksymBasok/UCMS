using UCMS.Domain.Courses;

namespace UCMS.Application.Abstractions.Repositories;

public interface ICourseRepository
{
    Task<Course> AddAsync(Course course, CancellationToken ct);
    Task<Course?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Course?> GetByCodeAsync(string code, CancellationToken ct);
    Task<bool> IsCodeUniqueAsync(string code, CancellationToken ct);
    Task<Course> UpdateAsync(Course course, CancellationToken ct);
    Task<Course> RemoveAsync(Course course, CancellationToken ct);
}
