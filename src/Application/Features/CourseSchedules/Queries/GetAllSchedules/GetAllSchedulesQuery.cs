using MediatR;
using UCMS.Application.Features.CourseSchedules.Dtos;

namespace UCMS.Application.Features.CourseSchedules.Queries.GetAllSchedules;
public sealed record GetAllSchedulesQuery() : IRequest<IReadOnlyList<CourseScheduleDto>>;
