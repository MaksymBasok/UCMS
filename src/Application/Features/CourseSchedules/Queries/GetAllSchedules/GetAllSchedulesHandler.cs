using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.CourseSchedules.Dtos;

namespace UCMS.Application.Features.CourseSchedules.Queries.GetAllSchedules;
public sealed class GetAllSchedulesHandler : IRequestHandler<GetAllSchedulesQuery, IReadOnlyList<CourseScheduleDto>>
{
    private readonly ICourseScheduleQueries _q; public GetAllSchedulesHandler(ICourseScheduleQueries q) => _q = q;
    public Task<IReadOnlyList<CourseScheduleDto>> Handle(GetAllSchedulesQuery r, CancellationToken ct) => _q.GetAllAsync(ct);
}
