using LanguageExt;
using MediatR;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Application.Features.Courses.Exceptions;

namespace UCMS.Application.Features.Courses.Commands.CreateCourse;

public sealed record CreateCourseCommand(string Code, string Title, string Description, int Credits)
    : IRequest<Either<CourseException, CourseDto>>;
