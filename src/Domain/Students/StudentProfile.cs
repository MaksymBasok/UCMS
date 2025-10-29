namespace UCMS.Domain.Students;

public sealed class StudentProfile
{
    public Guid StudentId { get; }
    public DateTime? DateOfBirth { get; private set; }
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private StudentProfile(Guid studentId, DateTime? dateOfBirth, string? address, string? phone, DateTime createdAt, DateTime? updatedAt)
    {
        StudentId = studentId;
        DateOfBirth = dateOfBirth;
        Address = address;
        Phone = phone;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static StudentProfile New(Guid studentId, DateTime? dob, string? address, string? phone)
    {
        if (studentId == Guid.Empty) throw new ArgumentException("StudentId");

        return new StudentProfile(
            studentId,
            dob,
            address?.Trim(),
            phone?.Trim(),
            DateTime.UtcNow,
            null);
    }

    public void Update(string? address, string? phone, DateTime? dob)
    {
        Address = address?.Trim();
        Phone = phone?.Trim();
        DateOfBirth = dob;
        UpdatedAt = DateTime.UtcNow;
    }
}
