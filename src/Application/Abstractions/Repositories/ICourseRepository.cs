using UCMS.Domain.Courses;

namespace UCMS.Application.Abstractions.Repositories;
public interface ICourseRepository
{
    Task AddAsync(Course course, CancellationToken ct);
    Task<Course?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Course?> GetByCodeAsync(string code, CancellationToken ct);
    Task RemoveAsync(Course course, CancellationToken ct);
}
