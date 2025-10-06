using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Queries;

public sealed class CourseScheduleQueries : ICourseScheduleQueries
{
    private readonly ApplicationDbContext _db;
    public CourseScheduleQueries(ApplicationDbContext db) => _db = db;

    public Task<IReadOnlyList<CourseScheduleDto>> GetAllAsync(CancellationToken ct) =>
        _db.CourseSchedules.AsNoTracking()
           .Select(s => new CourseScheduleDto(s.Id, s.CourseId, s.Topic, s.Frequency, s.NextSessionDate, s.IsActive))
           .ToListAsync(ct)
           .ContinueWith(t => (IReadOnlyList<CourseScheduleDto>)t.Result, ct);

    public Task<IReadOnlyList<CourseScheduleDto>> GetByCourseAsync(Guid courseId, CancellationToken ct) =>
        _db.CourseSchedules.AsNoTracking()
           .Where(s => s.CourseId == courseId)
           .Select(s => new CourseScheduleDto(s.Id, s.CourseId, s.Topic, s.Frequency, s.NextSessionDate, s.IsActive))
           .ToListAsync(ct)
           .ContinueWith(t => (IReadOnlyList<CourseScheduleDto>)t.Result, ct);
}
