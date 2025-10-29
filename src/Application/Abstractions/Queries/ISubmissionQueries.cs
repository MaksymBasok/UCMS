using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Abstractions.Queries;

public interface ISubmissionQueries
{
    Task<IReadOnlyList<SubmissionDto>> GetAllAsync(CancellationToken ct);
    Task<SubmissionDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<SubmissionDto?> GetByAssignmentAndStudentAsync(Guid assignmentId, Guid studentId, CancellationToken ct);
}
