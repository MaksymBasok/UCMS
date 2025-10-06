using MediatR;
using UCMS.Application.Features.Students.Dtos;

namespace UCMS.Application.Features.Students.Commands.CreateStudent;

public sealed record CreateStudentCommand(
    string StudentNumber,
    string FullName,
    string Email,
    Guid GroupId
) : IRequest<StudentDto>;
