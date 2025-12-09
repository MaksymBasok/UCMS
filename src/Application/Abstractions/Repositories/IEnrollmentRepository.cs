using UCMS.Domain.Enrollments;

namespace UCMS.Application.Abstractions.Repositories;

public interface IEnrollmentRepository
{
    Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken ct);
    Task<Enrollment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct);
    Task<Enrollment> UpdateAsync(Enrollment enrollment, CancellationToken ct);
}
