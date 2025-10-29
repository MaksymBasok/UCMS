using UCMS.Application.Features.Courses.Dtos;
namespace UCMS.Application.Abstractions.Queries;
public interface ICourseQueries
{
    Task<IReadOnlyList<CourseDto>> GetAllAsync(CancellationToken ct);
    Task<CourseDto?> GetByIdAsync(Guid id, CancellationToken ct);
}
