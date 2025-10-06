namespace UCMS.Domain.Students;

public sealed class Student
{
    public Guid Id { get; private set; }
    public string StudentNumber { get; private set; } = default!;
    public string FullName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public Guid GroupId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Student() { }

    public static Student New(Guid id, string studentNumber, string fullName, string email, Guid groupId)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (string.IsNullOrWhiteSpace(studentNumber) || studentNumber.Length > 20) throw new ArgumentException("StudentNumber");
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length > 100) throw new ArgumentException("FullName");
        if (string.IsNullOrWhiteSpace(email) || email.Length > 200) throw new ArgumentException("Email");
        if (groupId == Guid.Empty) throw new ArgumentException("GroupId");

        return new Student
        {
            Id = id,
            StudentNumber = studentNumber.Trim(),
            FullName = fullName.Trim(),
            Email = email.Trim(),
            GroupId = groupId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string fullName, string email, Guid groupId)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length > 100) throw new ArgumentException("FullName");
        if (string.IsNullOrWhiteSpace(email) || email.Length > 200) throw new ArgumentException("Email");
        if (groupId == Guid.Empty) throw new ArgumentException("GroupId");

        FullName = fullName.Trim();
        Email = email.Trim();
        GroupId = groupId;
        UpdatedAt = DateTime.UtcNow;
    }
}
