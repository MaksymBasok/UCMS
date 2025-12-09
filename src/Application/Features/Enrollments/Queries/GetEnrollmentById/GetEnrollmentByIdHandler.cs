using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Features.Enrollments.Queries.GetEnrollmentById;

public sealed class GetEnrollmentByIdHandler : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentDto?>
{
    private readonly IEnrollmentQueries _queries;

    public GetEnrollmentByIdHandler(IEnrollmentQueries queries)
    {
        _queries = queries;
    }

    public Task<EnrollmentDto?> Handle(GetEnrollmentByIdQuery request, CancellationToken ct)
        => _queries.GetByIdAsync(request.Id, ct);
}
