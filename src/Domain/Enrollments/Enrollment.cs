namespace UCMS.Domain.Enrollments;

public sealed class Enrollment
{
    public Guid Id { get; private set; }
    public Guid StudentId { get; private set; }
    public Guid CourseId { get; private set; }
    public EnrollmentStatus Status { get; private set; }
    public DateTime EnrolledAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Enrollment() { }

    public static Enrollment New(Guid id, Guid studentId, Guid courseId)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (studentId == Guid.Empty) throw new ArgumentException("StudentId");
        if (courseId == Guid.Empty) throw new ArgumentException("CourseId");

        return new Enrollment
        {
            Id = id,
            StudentId = studentId,
            CourseId = courseId,
            Status = EnrollmentStatus.Active,
            EnrolledAt = DateTime.UtcNow
        };
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
