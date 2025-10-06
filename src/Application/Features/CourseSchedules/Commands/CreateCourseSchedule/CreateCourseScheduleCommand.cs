using MediatR;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Domain.Schedules;

namespace UCMS.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;
public sealed record CreateCourseScheduleCommand(Guid CourseId, string Topic, CourseScheduleFrequency Frequency, DateTime NextSessionDate) : IRequest<CourseScheduleDto>;
