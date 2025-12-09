using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Features.Enrollments.Queries.GetEnrollments;

public sealed class GetEnrollmentsHandler : IRequestHandler<GetEnrollmentsQuery, IReadOnlyList<EnrollmentDto>>
{
    private readonly IEnrollmentQueries _queries;

    public GetEnrollmentsHandler(IEnrollmentQueries queries)
    {
        _queries = queries;
    }

    public Task<IReadOnlyList<EnrollmentDto>> Handle(GetEnrollmentsQuery request, CancellationToken ct)
    {
        if (request.StudentId is Guid studentId)
        {
            return _queries.GetByStudentAsync(studentId, ct);
        }

        if (request.CourseId is Guid courseId)
        {
            return _queries.GetByCourseAsync(courseId, ct);
        }

        return _queries.GetAllAsync(ct);
    }
}
