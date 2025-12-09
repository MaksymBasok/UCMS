using UCMS.Domain.Enrollments;

namespace UCMS.Application.Features.Enrollments.Dtos;

public sealed record EnrollmentDto(
    Guid Id,
    Guid StudentId,
    Guid CourseId,
    EnrollmentStatus Status,
    DateTime EnrolledAt,
    DateTime? CompletedAt)
{
    public static EnrollmentDto From(Enrollment e)
        => new(e.Id, e.StudentId, e.CourseId, e.Status, e.EnrolledAt, e.CompletedAt);
}
