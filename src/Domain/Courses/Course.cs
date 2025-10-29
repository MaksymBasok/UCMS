namespace UCMS.Domain.Courses;

public sealed class Course
{
    public Guid Id { get; }
    public string Code { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int Credits { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private Course(Guid id, string code, string title, string description, int credits, DateTime createdAt, DateTime? updatedAt)
    {
        Id = id;
        Code = code;
        Title = title;
        Description = description;
        Credits = credits;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Course New(Guid id, string code, string title, string description, int credits)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (string.IsNullOrWhiteSpace(code) || code.Length > 20) throw new ArgumentException("Code");
        if (string.IsNullOrWhiteSpace(title) || title.Length > 200) throw new ArgumentException("Title");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 1000) throw new ArgumentException("Description");
        if (credits is < 1 or > 60) throw new ArgumentException("Credits");

        return new Course(
            id,
            code.Trim(),
            title.Trim(),
            description.Trim(),
            credits,
            DateTime.UtcNow,
            null);
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
