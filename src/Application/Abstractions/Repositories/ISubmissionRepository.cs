using UCMS.Domain.Submissions;

namespace UCMS.Application.Abstractions.Repositories;

public interface ISubmissionRepository
{
    Task<Submission> AddAsync(Submission submission, CancellationToken ct);
    Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Submission> UpdateAsync(Submission submission, CancellationToken ct);
    Task<Submission> RemoveAsync(Submission submission, CancellationToken ct);
}
