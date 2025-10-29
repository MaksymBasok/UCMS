using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Application.Features.Students.Exceptions;
using UCMS.Domain.Students;

namespace UCMS.Application.Features.Students.Commands.CreateStudent;

public sealed class CreateStudentHandler
    : IRequestHandler<CreateStudentCommand, Either<StudentException, StudentDto>>
{
    private readonly IStudentRepository _repo;

    public CreateStudentHandler(IStudentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Either<StudentException, StudentDto>> Handle(
        CreateStudentCommand request,
        CancellationToken ct)
    {
        try
        {
            var existing = await _repo.GetByStudentNumberAsync(request.StudentNumber, ct);
            if (existing is not null)
            {
                return new StudentAlreadyExistsException(existing.Id, existing.StudentNumber);
            }

            var student = Student.New(
                Guid.NewGuid(),
                request.StudentNumber,
                request.FullName,
                request.Email,
                request.GroupId);

            await _repo.AddAsync(student, ct);

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
