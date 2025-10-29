using UCMS.Domain.Schedules;

namespace UCMS.Application.Abstractions.Repositories;

public interface ICourseScheduleRepository
{
    Task<CourseSchedule> AddAsync(CourseSchedule schedule, CancellationToken ct);
    Task<CourseSchedule?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<CourseSchedule> UpdateAsync(CourseSchedule schedule, CancellationToken ct);
}
