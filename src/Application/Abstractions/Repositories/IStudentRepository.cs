using UCMS.Domain.Students;

namespace UCMS.Application.Abstractions.Repositories;

public interface IStudentRepository
{
    Task AddAsync(Student student, CancellationToken ct);
    Task<Student?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> IsStudentNumberUniqueAsync(string studentNumber, CancellationToken ct);
}
