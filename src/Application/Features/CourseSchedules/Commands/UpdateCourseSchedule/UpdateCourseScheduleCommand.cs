using MediatR;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Domain.Schedules;

namespace UCMS.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule;
public sealed record UpdateCourseScheduleCommand(Guid Id, string Topic, CourseScheduleFrequency Frequency, DateTime NextSessionDate) : IRequest<CourseScheduleDto>;
