using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Students.Exceptions;

namespace UCMS.Application.Features.Students.Commands.DeleteStudent;

public sealed class DeleteStudentHandler
    : IRequestHandler<DeleteStudentCommand, Either<StudentException, LanguageExt.Unit>>
{
    private readonly IStudentRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteStudentHandler(IStudentRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Either<StudentException, LanguageExt.Unit>> Handle(
        DeleteStudentCommand request,
        CancellationToken ct)
    {
        try
        {
            var student = await _repo.GetByIdAsync(request.Id, ct);
            if (student is null)
            {
                return new StudentNotFoundException(request.Id);
            }

            await _repo.RemoveAsync(student, ct);
            await _uow.SaveChangesAsync(ct);

            return LanguageExt.Unit.Default;
        }
        catch (Exception ex)
        {
            return new StudentUnexpectedException(request.Id, ex);
        }
    }
}
