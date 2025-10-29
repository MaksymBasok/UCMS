using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Schedules;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class CourseScheduleRepository : ICourseScheduleRepository
{
    private readonly ApplicationDbContext _db;

    public CourseScheduleRepository(ApplicationDbContext db) => _db = db;

    public async Task<CourseSchedule> AddAsync(CourseSchedule schedule, CancellationToken ct)
    {
        await _db.CourseSchedules.AddAsync(schedule, ct);
        await _db.SaveChangesAsync(ct);

        return schedule;
    }

    public Task<CourseSchedule?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.CourseSchedules.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<CourseSchedule> UpdateAsync(CourseSchedule schedule, CancellationToken ct)
    {
        _db.CourseSchedules.Update(schedule);
        await _db.SaveChangesAsync(ct);

        return schedule;
    }
}
