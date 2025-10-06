using System;

namespace UCMS.Domain.Entities;

public sealed class Course
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Location { get; private set; } = default!;
    public CourseStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Course() { }

    public static Course New(Guid id, string code, string title, string description, string location)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id empty");
        if (string.IsNullOrWhiteSpace(code) || code.Length > 20) throw new ArgumentException("Code invalid");
        if (string.IsNullOrWhiteSpace(title) || title.Length > 100) throw new ArgumentException("Title invalid");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 500) throw new ArgumentException("Description invalid");
        if (string.IsNullOrWhiteSpace(location) || location.Length > 200) throw new ArgumentException("Location invalid");

        return new Course
        {
            Id = id,
            Code = code.Trim(),
            Title = title.Trim(),
            Description = description.Trim(),
            Location = location.Trim(),
            Status = CourseStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string title, string description, string location)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > 100) throw new ArgumentException("Title invalid");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 500) throw new ArgumentException("Description invalid");
        if (string.IsNullOrWhiteSpace(location) || location.Length > 200) throw new ArgumentException("Location invalid");

        Title = title.Trim();
        Description = description.Trim();
        Location = location.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(CourseStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum CourseStatus { Active, Suspended, Archived }
