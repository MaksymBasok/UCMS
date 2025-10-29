using UCMS.Application.Features.CourseSchedules.Dtos;
namespace UCMS.Application.Abstractions.Queries;
public interface ICourseScheduleQueries
{
    Task<IReadOnlyList<CourseScheduleDto>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<CourseScheduleDto>> GetByCourseAsync(Guid courseId, CancellationToken ct);
}
