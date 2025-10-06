using MediatR;
using UCMS.Application.Features.Courses.Dtos;

namespace UCMS.Application.Features.Courses.Queries.GetCourses;
public sealed record GetCoursesQuery() : IRequest<IReadOnlyList<CourseDto>>;
