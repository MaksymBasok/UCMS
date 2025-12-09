using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Abstractions.Queries;

public interface IEnrollmentQueries
{
    Task<IReadOnlyList<EnrollmentDto>> GetAllAsync(CancellationToken ct);
    Task<EnrollmentDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<EnrollmentDto>> GetByStudentAsync(Guid studentId, CancellationToken ct);
    Task<IReadOnlyList<EnrollmentDto>> GetByCourseAsync(Guid courseId, CancellationToken ct);
    Task<EnrollmentDto?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct);
}
