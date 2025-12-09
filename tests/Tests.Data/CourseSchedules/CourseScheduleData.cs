using UCMS.Domain.Schedules;

namespace Tests.Data.CourseSchedules;

public static class CourseScheduleData
{
    public static CourseSchedule FirstSchedule(Guid courseId)
        => CourseSchedule.New(
            new Guid("12345678-1234-1234-1234-1234567890ab"),
            courseId,
            "Weekly Sync",
            CourseScheduleFrequency.Weekly,
            DateTime.UtcNow.AddDays(3));

    public static CourseSchedule SecondSchedule(Guid courseId)
        => CourseSchedule.New(
            new Guid("abcdefab-cdef-abcd-efab-cdefabcdef12"),
            courseId,
            "Monthly Review",
            CourseScheduleFrequency.Monthly,
            DateTime.UtcNow.AddDays(30));
}
