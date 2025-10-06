using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Courses.Dtos;

namespace UCMS.Application.Features.Courses.Queries.GetCourseById;
public sealed class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, CourseDto?>
{
    private readonly ICourseQueries _q; public GetCourseByIdHandler(ICourseQueries q) => _q = q;
    public Task<CourseDto?> Handle(GetCourseByIdQuery r, CancellationToken ct) => _q.GetByIdAsync(r.Id, ct);
}
