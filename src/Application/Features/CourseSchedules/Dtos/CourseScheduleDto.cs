using UCMS.Domain.Schedules;

namespace UCMS.Application.Features.CourseSchedules.Dtos;
public sealed record CourseScheduleDto(Guid Id, Guid CourseId, string Topic, CourseScheduleFrequency Frequency, DateTime NextSessionDate, bool IsActive)
{
    public static CourseScheduleDto From(CourseSchedule s) => new(s.Id, s.CourseId, s.Topic, s.Frequency, s.NextSessionDate, s.IsActive);
}
