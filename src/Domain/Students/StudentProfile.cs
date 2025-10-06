namespace UCMS.Domain.Students;

public sealed class StudentProfile
{
    public Guid StudentId { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private StudentProfile() { }

    public static StudentProfile New(Guid studentId, DateTime? dob, string? address, string? phone)
    {
        if (studentId == Guid.Empty) throw new ArgumentException("StudentId");
        return new StudentProfile
        {
            StudentId = studentId,
            DateOfBirth = dob,
            Address = address?.Trim(),
            Phone = phone?.Trim(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string? address, string? phone, DateTime? dob)
    {
        Address = address?.Trim();
        Phone = phone?.Trim();
        DateOfBirth = dob;
        UpdatedAt = DateTime.UtcNow;
    }
}
