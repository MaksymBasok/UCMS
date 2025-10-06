using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Courses.Dtos;

namespace UCMS.Application.Features.Courses.Queries.GetCourses;
public sealed class GetCoursesHandler : IRequestHandler<GetCoursesQuery, IReadOnlyList<CourseDto>>
{
    private readonly ICourseQueries _q; public GetCoursesHandler(ICourseQueries q) => _q = q;
    public Task<IReadOnlyList<CourseDto>> Handle(GetCoursesQuery r, CancellationToken ct) => _q.GetAllAsync(ct);
}
