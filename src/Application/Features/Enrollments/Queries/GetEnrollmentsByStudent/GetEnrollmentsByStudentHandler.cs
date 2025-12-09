using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Features.Enrollments.Queries.GetEnrollmentsByStudent;

public sealed class GetEnrollmentsByStudentHandler
    : IRequestHandler<GetEnrollmentsByStudentQuery, IReadOnlyList<EnrollmentDto>>
{
    private readonly IEnrollmentQueries _queries;

    public GetEnrollmentsByStudentHandler(IEnrollmentQueries queries)
    {
        _queries = queries;
    }

    public Task<IReadOnlyList<EnrollmentDto>> Handle(GetEnrollmentsByStudentQuery request, CancellationToken ct)
        => _queries.GetByStudentAsync(request.StudentId, ct);
}
