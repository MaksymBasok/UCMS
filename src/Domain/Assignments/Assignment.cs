namespace UCMS.Domain.Assignments;

public sealed class Assignment
{
    public Guid Id { get; }
    public Guid CourseId { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public AssignmentStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private Assignment(Guid id, Guid courseId, string title, string description, DateTime dueDate, AssignmentStatus status, DateTime createdAt, DateTime? updatedAt)
    {
        Id = id;
        CourseId = courseId;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Assignment New(Guid id, Guid courseId, string title, string description, DateTime dueDate)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (courseId == Guid.Empty) throw new ArgumentException("CourseId");
        if (string.IsNullOrWhiteSpace(title) || title.Length > 200) throw new ArgumentException("Title");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 1000) throw new ArgumentException("Description");
        if (dueDate <= DateTime.UtcNow) throw new ArgumentException("DueDate");

        return new Assignment(
            id,
            courseId,
            title.Trim(),
            description.Trim(),
            dueDate,
            AssignmentStatus.Draft,
            DateTime.UtcNow,
            null);
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
