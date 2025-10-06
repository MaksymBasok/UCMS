using MediatR;
using UCMS.Application.Features.Students.Dtos;

namespace UCMS.Application.Features.Students.Queries.GetStudents;

public sealed record GetStudentsQuery() : IRequest<IReadOnlyList<StudentDto>>;
