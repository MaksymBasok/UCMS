using UCMS.Application.Abstractions.Repositories;
using UCMS.Domain.Submissions;
using UCMS.Infrastructure.Persistence;

namespace UCMS.Infrastructure.Repositories;

public sealed class SubmissionRepository : ISubmissionRepository
{
    private readonly ApplicationDbContext _db;
    public SubmissionRepository(ApplicationDbContext db) => _db = db;

    public Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct) => _db.Submissions.FindAsync([id], ct).AsTask();
    public Task AddAsync(Submission s, CancellationToken ct) { _db.Submissions.Add(s); return Task.CompletedTask; }
}
