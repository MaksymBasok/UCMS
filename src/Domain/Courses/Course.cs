namespace UCMS.Domain.Courses;

public sealed class Course
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Credits { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Course() { }

    public static Course New(Guid id, string code, string title, string description, int credits)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (string.IsNullOrWhiteSpace(code) || code.Length > 20) throw new ArgumentException("Code");
        if (string.IsNullOrWhiteSpace(title) || title.Length > 200) throw new ArgumentException("Title");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 1000) throw new ArgumentException("Description");
        if (credits is < 1 or > 60) throw new ArgumentException("Credits");

        return new Course
        {
            Id = id,
            Code = code.Trim(),
            Title = title.Trim(),
            Description = description.Trim(),
            Credits = credits,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string title, string description, int credits)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > 200) throw new ArgumentException("Title");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 1000) throw new ArgumentException("Description");
        if (credits is < 1 or > 60) throw new ArgumentException("Credits");

        Title = title.Trim();
        Description = description.Trim();
        Credits = credits;
        UpdatedAt = DateTime.UtcNow;
    }
}
