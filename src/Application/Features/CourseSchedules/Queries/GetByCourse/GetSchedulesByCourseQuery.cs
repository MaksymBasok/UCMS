using MediatR;
using UCMS.Application.Features.CourseSchedules.Dtos;

namespace UCMS.Application.Features.CourseSchedules.Queries.GetByCourse;
public sealed record GetSchedulesByCourseQuery(Guid CourseId) : IRequest<IReadOnlyList<CourseScheduleDto>>;
