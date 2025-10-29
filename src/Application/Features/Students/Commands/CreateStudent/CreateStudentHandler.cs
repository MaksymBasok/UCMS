using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Application.Features.Students.Exceptions;
using UCMS.Domain.Students;

namespace UCMS.Application.Features.Students.Commands.CreateStudent;

public sealed class CreateStudentHandler
    : IRequestHandler<CreateStudentCommand, Either<StudentException, StudentDto>>
{
    private readonly IStudentRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateStudentHandler(IStudentRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Either<StudentException, StudentDto>> Handle(
        CreateStudentCommand request,
        CancellationToken ct)
    {
        try
        {
            var normalizedNumber = request.StudentNumber.Trim();
            var existing = await _repo.GetByStudentNumberAsync(normalizedNumber, ct);
            if (existing is not null)
            {
                return new StudentAlreadyExistsException(existing.Id, normalizedNumber);
            }

            var student = Student.New(Guid.NewGuid(), normalizedNumber, request.FullName, request.Email, request.GroupId);
            await _repo.AddAsync(student, ct);
            await _uow.SaveChangesAsync(ct);

            return StudentDto.From(student);
        }
        catch (ArgumentException ex)
        {
            return new StudentValidationException(Guid.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            return new StudentUnexpectedException(Guid.Empty, ex);
        }
    }
}
