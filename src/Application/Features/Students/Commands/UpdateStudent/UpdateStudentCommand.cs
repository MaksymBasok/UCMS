using LanguageExt;
using MediatR;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Application.Features.Students.Exceptions;

namespace UCMS.Application.Features.Students.Commands.UpdateStudent;

public sealed record UpdateStudentCommand(
    Guid Id,
    string FullName,
    string Email,
    Guid GroupId
) : IRequest<Either<StudentException, StudentDto>>;
