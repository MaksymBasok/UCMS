using UCMS.Domain.Submissions;
namespace UCMS.Application.Abstractions.Repositories;
public interface ISubmissionRepository
{
    Task AddAsync(Submission submission, CancellationToken ct);
    Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct);
}
