using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Students.Dtos;

namespace UCMS.Application.Features.Students.Queries.GetStudents;

public sealed class GetStudentsHandler : IRequestHandler<GetStudentsQuery, IReadOnlyList<StudentDto>>
{
    private readonly IStudentQueries _queries;
    public GetStudentsHandler(IStudentQueries queries) => _queries = queries;

    public Task<IReadOnlyList<StudentDto>> Handle(GetStudentsQuery request, CancellationToken ct)
        => _queries.GetAllAsync(ct);
}
