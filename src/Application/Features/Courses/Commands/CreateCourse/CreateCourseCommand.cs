using MediatR;
using UCMS.Application.Features.Courses.Dtos;

namespace UCMS.Application.Features.Courses.Commands.CreateCourse;
public sealed record CreateCourseCommand(string Code, string Title, string Description, int Credits) : IRequest<CourseDto>;
