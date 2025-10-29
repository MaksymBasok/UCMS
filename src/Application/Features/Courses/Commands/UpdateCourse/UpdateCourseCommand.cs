using LanguageExt;
using MediatR;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Application.Features.Courses.Exceptions;

namespace UCMS.Application.Features.Courses.Commands.UpdateCourse;

public sealed record UpdateCourseCommand(Guid Id, string Title, string Description, int Credits)
    : IRequest<Either<CourseException, CourseDto>>;
