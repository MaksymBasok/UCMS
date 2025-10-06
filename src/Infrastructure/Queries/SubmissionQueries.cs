using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Submissions.Dtos;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Queries;

public sealed class SubmissionQueries : ISubmissionQueries
{
    private readonly ApplicationDbContext _db;
    public SubmissionQueries(ApplicationDbContext db) => _db = db;

    public Task<IReadOnlyList<SubmissionDto>> GetAllAsync(CancellationToken ct) =>
        _db.Submissions.AsNoTracking()
           .Select(s => new SubmissionDto(s.Id, s.AssignmentId, s.StudentId, s.ContentUrl, s.Status, s.SubmittedAt, s.Grade, s.CompletionNotes))
           .ToListAsync(ct)
           .ContinueWith(t => (IReadOnlyList<SubmissionDto>)t.Result, ct);

    public Task<SubmissionDto?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Submissions.AsNoTracking()
           .Where(s => s.Id == id)
           .Select(s => new SubmissionDto(s.Id, s.AssignmentId, s.StudentId, s.ContentUrl, s.Status, s.SubmittedAt, s.Grade, s.CompletionNotes))
           .FirstOrDefaultAsync(ct);
}
