using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Students.Dtos;

namespace UCMS.Application.Features.Students.Queries.GetStudentById;

public sealed class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, StudentDto?>
{
    private readonly IStudentQueries _queries;

    public GetStudentByIdHandler(IStudentQueries queries) => _queries = queries;

    public Task<StudentDto?> Handle(GetStudentByIdQuery request, CancellationToken ct)
        => _queries.GetByIdAsync(request.Id, ct);
}
