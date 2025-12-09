namespace UCMS.Domain.Students;

public sealed class StudentProfile
{
    public const int AddressMaxLength = 500;
    public const int PhoneMaxLength = 30;

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
        if (address?.Length > AddressMaxLength) throw new ArgumentException("Address");
        if (phone?.Length > PhoneMaxLength) throw new ArgumentException("Phone");

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
        if (address?.Length > AddressMaxLength) throw new ArgumentException("Address");
        if (phone?.Length > PhoneMaxLength) throw new ArgumentException("Phone");

        Address = address?.Trim();
        Phone = phone?.Trim();
        DateOfBirth = dob;
        UpdatedAt = DateTime.UtcNow;
    }
}
