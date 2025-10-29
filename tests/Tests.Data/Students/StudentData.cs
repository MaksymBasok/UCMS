using UCMS.Domain.Students;

namespace Tests.Data.Students;

public static class StudentData
{
    public static Student FirstStudent()
        => Student.New(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "STU-001",
            "John Doe",
            "john.doe@example.com",
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

    public static Student SecondStudent()
        => Student.New(
            Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            "STU-002",
            "Jane Smith",
            "jane.smith@example.com",
            Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));
}
