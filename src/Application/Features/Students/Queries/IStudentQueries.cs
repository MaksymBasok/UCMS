using UCMS.Application.Features.Students.Dtos;

namespace UCMS.Application.Abstractions.Queries;

public interface IStudentQueries
{
    Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken ct);
    Task<StudentDto?> GetByIdAsync(Guid id, CancellationToken ct);
}
