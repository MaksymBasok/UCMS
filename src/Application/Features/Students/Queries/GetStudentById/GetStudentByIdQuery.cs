using MediatR;
using UCMS.Application.Features.Students.Dtos;

namespace UCMS.Application.Features.Students.Queries.GetStudentById;

public sealed record GetStudentByIdQuery(Guid Id) : IRequest<StudentDto?>;
