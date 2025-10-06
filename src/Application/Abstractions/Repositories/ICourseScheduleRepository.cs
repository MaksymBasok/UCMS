using UCMS.Domain.Schedules;
namespace UCMS.Application.Abstractions.Repositories;
public interface ICourseScheduleRepository
{
    Task AddAsync(CourseSchedule schedule, CancellationToken ct);
    Task<CourseSchedule?> GetByIdAsync(Guid id, CancellationToken ct);
}
