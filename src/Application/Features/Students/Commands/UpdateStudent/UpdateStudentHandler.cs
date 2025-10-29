using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Application.Features.Students.Exceptions;

namespace UCMS.Application.Features.Students.Commands.UpdateStudent;

public sealed class UpdateStudentHandler
    : IRequestHandler<UpdateStudentCommand, Either<StudentException, StudentDto>>
{
    private readonly IStudentRepository _repo;

    public UpdateStudentHandler(IStudentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Either<StudentException, StudentDto>> Handle(
        UpdateStudentCommand request,
        CancellationToken ct)
    {
        try
        {
            var student = await _repo.GetByIdAsync(request.Id, ct);
            if (student is null)
            {
                return new StudentNotFoundException(request.Id);
            }

            student.UpdateProfile(request.FirstName, request.LastName, request.Email);
            await _repo.UpdateAsync(student, ct);

            return StudentDto.From(student);
        }
        catch (ArgumentException ex)
        {
            return new StudentValidationException(request.Id, ex.Message);
        }
        catch (Exception ex)
        {
            return new StudentUnexpectedException(request.Id, ex);
        }
    }
}
