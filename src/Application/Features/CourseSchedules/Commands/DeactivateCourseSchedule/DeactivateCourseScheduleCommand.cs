using MediatR;

namespace UCMS.Application.Features.CourseSchedules.Commands.DeactivateCourseSchedule;

public sealed record DeactivateCourseScheduleCommand(Guid Id) : IRequest<MediatR.Unit>;
