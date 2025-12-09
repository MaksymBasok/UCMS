using UCMS.Domain.Enrollments;

namespace Tests.Data.Enrollments;

public static class EnrollmentData
{
    public static Enrollment ActiveEnrollment(Guid studentId, Guid courseId)
        => Enrollment.New(
            new Guid("e0f8c0c6-6f7a-4e6c-8b2e-8f9d5cf7c7e1"),
            studentId,
            courseId);

    public static Enrollment CompletedEnrollment(Guid studentId, Guid courseId)
    {
        var enrollment = Enrollment.New(
            new Guid("2f7d5c6e-9b7a-42f2-a2a8-9e3b7c1d2f5a"),
            studentId,
            courseId);
        enrollment.Complete();
        return enrollment;
    }
}
