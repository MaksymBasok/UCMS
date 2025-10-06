using MediatR;
using UCMS.Application.Features.Courses.Dtos;

namespace UCMS.Application.Features.Courses.Queries.GetCourseById;
public sealed record GetCourseByIdQuery(Guid Id) : IRequest<CourseDto?>;
