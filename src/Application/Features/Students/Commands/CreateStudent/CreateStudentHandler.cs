using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Domain.Students;

namespace UCMS.Application.Features.Students.Commands.CreateStudent;

public sealed class CreateStudentHandler : IRequestHandler<CreateStudentCommand, StudentDto>
{
    private readonly IStudentRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateStudentHandler(IStudentRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken ct)
    {
        if (!await _repo.IsStudentNumberUniqueAsync(request.StudentNumber, ct))
            throw new InvalidOperationException("StudentNumber is not unique");

        var student = Student.New(Guid.NewGuid(), request.StudentNumber, request.FullName, request.Email, request.GroupId);
        await _repo.AddAsync(student, ct);
        await _uow.SaveChangesAsync(ct);
        return StudentDto.From(student);
    }
}
