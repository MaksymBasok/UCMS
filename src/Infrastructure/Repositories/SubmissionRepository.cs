using Microsoft.EntityFrameworkCore;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Submissions;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class SubmissionRepository : ISubmissionRepository
{
    private readonly ApplicationDbContext _db;

    public SubmissionRepository(ApplicationDbContext db) => _db = db;

    public async Task<Submission> AddAsync(Submission submission, CancellationToken ct)
    {
        await _db.Submissions.AddAsync(submission, ct);
        await _db.SaveChangesAsync(ct);

        return submission;
    }

    public Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Submissions.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<Submission> UpdateAsync(Submission submission, CancellationToken ct)
    {
        _db.Submissions.Update(submission);
        await _db.SaveChangesAsync(ct);

        return submission;
    }
}
