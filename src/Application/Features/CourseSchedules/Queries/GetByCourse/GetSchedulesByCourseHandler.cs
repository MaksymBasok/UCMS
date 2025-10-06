using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.CourseSchedules.Dtos;

namespace UCMS.Application.Features.CourseSchedules.Queries.GetByCourse;
public sealed class GetSchedulesByCourseHandler : IRequestHandler<GetSchedulesByCourseQuery, IReadOnlyList<CourseScheduleDto>>
{
    private readonly ICourseScheduleQueries _q; public GetSchedulesByCourseHandler(ICourseScheduleQueries q) => _q = q;
    public Task<IReadOnlyList<CourseScheduleDto>> Handle(GetSchedulesByCourseQuery r, CancellationToken ct) => _q.GetByCourseAsync(r.CourseId, ct);
}
