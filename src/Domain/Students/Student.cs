namespace UCMS.Domain.Students;

public sealed class Student
{
    public Guid Id { get; }
    public string StudentNumber { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public Guid GroupId { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private Student(Guid id, string studentNumber, string fullName, string email, Guid groupId, DateTime createdAt, DateTime? updatedAt)
    {
        Id = id;
        StudentNumber = studentNumber;
        FullName = fullName;
        Email = email;
        GroupId = groupId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Student New(Guid id, string studentNumber, string fullName, string email, Guid groupId)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (string.IsNullOrWhiteSpace(studentNumber) || studentNumber.Length > 20) throw new ArgumentException("StudentNumber");
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length > 100) throw new ArgumentException("FullName");
        if (string.IsNullOrWhiteSpace(email) || email.Length > 200) throw new ArgumentException("Email");
        if (groupId == Guid.Empty) throw new ArgumentException("GroupId");

        return new Student(
            id,
            studentNumber.Trim(),
            fullName.Trim(),
            email.Trim(),
            groupId,
            DateTime.UtcNow,
            null);
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
