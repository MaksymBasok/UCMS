using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Students.Exceptions;

namespace UCMS.Application.Features.Students.Commands.DeleteStudent;

public sealed class DeleteStudentHandler
    : IRequestHandler<DeleteStudentCommand, LanguageExt.Either<StudentException, LanguageExt.Unit>>
{
    private readonly IStudentRepository _repo;

    public DeleteStudentHandler(IStudentRepository repo)
    {
        _repo = repo;
    }

    public async Task<LanguageExt.Either<StudentException, LanguageExt.Unit>> Handle(
        DeleteStudentCommand request,
        CancellationToken ct)
    {
        var student = await _repo.GetByIdAsync(request.Id, ct);
        if (student is null)
        {
            return new StudentNotFoundException(request.Id);
        }

        await _repo.RemoveAsync(student, ct);

        return LanguageExt.Unit.Default;
    }
}
