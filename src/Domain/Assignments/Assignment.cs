namespace UCMS.Domain.Assignments;

public sealed class Assignment
{
    public Guid Id { get; private set; }
    public Guid CourseId { get; private set; }
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public DateTime DueDate { get; private set; }
    public AssignmentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Assignment() { }

    public static Assignment New(Guid id, Guid courseId, string title, string description, DateTime dueDate)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (courseId == Guid.Empty) throw new ArgumentException("CourseId");
        if (string.IsNullOrWhiteSpace(title) || title.Length > 200) throw new ArgumentException("Title");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 1000) throw new ArgumentException("Description");
        if (dueDate <= DateTime.UtcNow) throw new ArgumentException("DueDate");

        return new Assignment
        {
            Id = id,
            CourseId = courseId,
            Title = title.Trim(),
            Description = description.Trim(),
            DueDate = dueDate,
            Status = AssignmentStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Publish()
    {
        if (Status != AssignmentStatus.Draft) throw new InvalidOperationException("Not Draft");
        Status = AssignmentStatus.Published;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status != AssignmentStatus.Published) throw new InvalidOperationException("Not Published");
        Status = AssignmentStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, DateTime dueDate)
    {
        if (Status == AssignmentStatus.Closed) throw new InvalidOperationException("Closed");
        if (string.IsNullOrWhiteSpace(title) || title.Length > 200) throw new ArgumentException("Title");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 1000) throw new ArgumentException("Description");
        if (dueDate <= DateTime.UtcNow) throw new ArgumentException("DueDate");

        Title = title.Trim();
        Description = description.Trim();
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum AssignmentStatus { Draft, Published, Closed }
