namespace UCMS.Domain.Enrollments;

public sealed class Enrollment
{
    public Guid Id { get; }
    public Guid StudentId { get; }
    public Guid CourseId { get; }
    public EnrollmentStatus Status { get; private set; }
    public DateTime EnrolledAt { get; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Enrollment(Guid id, Guid studentId, Guid courseId, EnrollmentStatus status, DateTime enrolledAt, DateTime? completedAt, DateTime? updatedAt)
    {
        Id = id;
        StudentId = studentId;
        CourseId = courseId;
        Status = status;
        EnrolledAt = enrolledAt;
        CompletedAt = completedAt;
        UpdatedAt = updatedAt;
    }

    public static Enrollment New(Guid id, Guid studentId, Guid courseId)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (studentId == Guid.Empty) throw new ArgumentException("StudentId");
        if (courseId == Guid.Empty) throw new ArgumentException("CourseId");

        return new Enrollment(
            id,
            studentId,
            courseId,
            EnrollmentStatus.Active,
            DateTime.UtcNow,
            null,
            null);
    }

    public void Complete()
    {
        if (Status != EnrollmentStatus.Active) throw new InvalidOperationException("Not Active");
        Status = EnrollmentStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = CompletedAt;
    }

    public void Drop()
    {
        if (Status == EnrollmentStatus.Completed) throw new InvalidOperationException("Already completed");
        Status = EnrollmentStatus.Dropped;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum EnrollmentStatus { Active, Dropped, Completed }
