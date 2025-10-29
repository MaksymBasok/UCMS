using LanguageExt;
using MediatR;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Application.Features.Students.Exceptions;

namespace UCMS.Application.Features.Students.Commands.CreateStudent;

public sealed record CreateStudentCommand(
    string StudentNumber,
    string FullName,
    string Email,
    Guid GroupId
) : IRequest<Either<StudentException, StudentDto>>;
