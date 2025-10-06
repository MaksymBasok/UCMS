using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Schedules;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class CourseScheduleRepository : ICourseScheduleRepository
{
    private readonly ApplicationDbContext _db;
    public CourseScheduleRepository(ApplicationDbContext db) => _db = db;

    public Task<CourseSchedule?> GetByIdAsync(Guid id, CancellationToken ct) => _db.CourseSchedules.FindAsync([id], ct).AsTask();
    public Task AddAsync(CourseSchedule schedule, CancellationToken ct) { _db.CourseSchedules.Add(schedule); return Task.CompletedTask; }
}
