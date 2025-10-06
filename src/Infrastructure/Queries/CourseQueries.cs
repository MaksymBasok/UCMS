using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Queries;

public sealed class CourseQueries : ICourseQueries
{
    private readonly ApplicationDbContext _db;
    public CourseQueries(ApplicationDbContext db) => _db = db;

    public Task<IReadOnlyList<CourseDto>> GetAllAsync(CancellationToken ct) =>
        _db.Courses.AsNoTracking()
           .Select(c => new CourseDto(c.Id, c.Code, c.Title, c.Description, c.Credits))
           .ToListAsync(ct)
           .ContinueWith(t => (IReadOnlyList<CourseDto>)t.Result, ct);

    public Task<CourseDto?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Courses.AsNoTracking()
           .Where(c => c.Id == id)
           .Select(c => new CourseDto(c.Id, c.Code, c.Title, c.Description, c.Credits))
           .FirstOrDefaultAsync(ct);
}
