using UCMS.Domain.Courses;

namespace Tests.Data.Courses;

public static class CourseData
{
    public static Course FirstCourse()
        => Course.New(
            new Guid("1d6f5115-9b65-41b7-8fda-f5d9f6e0b2b1"),
            "TEST-101",
            "Integration Testing",
            "First integration testing course",
            5);

    public static Course SecondCourse()
        => Course.New(
            new Guid("5d9c9c8a-2c9c-4a07-8231-5ae7b6f83754"),
            "TEST-102",
            "Advanced Integration",
            "Second integration testing course",
            6);
}
