using UCMS.Domain.Students;

namespace UCMS.Application.Features.Students.Dtos;

public sealed record StudentDto(Guid Id, string StudentNumber, string FullName, string Email, Guid GroupId)
{
    public static StudentDto From(Student s) => new(s.Id, s.StudentNumber, s.FullName, s.Email, s.GroupId);
}
