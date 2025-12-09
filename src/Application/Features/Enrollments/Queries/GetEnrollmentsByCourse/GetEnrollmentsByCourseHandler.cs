using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Features.Enrollments.Queries.GetEnrollmentsByCourse;

public sealed class GetEnrollmentsByCourseHandler
    : IRequestHandler<GetEnrollmentsByCourseQuery, IReadOnlyList<EnrollmentDto>>
{
    private readonly IEnrollmentQueries _queries;

    public GetEnrollmentsByCourseHandler(IEnrollmentQueries queries)
    {
        _queries = queries;
    }

    public Task<IReadOnlyList<EnrollmentDto>> Handle(GetEnrollmentsByCourseQuery request, CancellationToken ct)
        => _queries.GetByCourseAsync(request.CourseId, ct);
}
