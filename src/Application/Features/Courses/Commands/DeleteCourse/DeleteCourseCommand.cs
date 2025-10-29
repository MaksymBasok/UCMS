using LanguageExt;
using MediatR;
using UCMS.Application.Features.Courses.Exceptions;

namespace UCMS.Application.Features.Courses.Commands.DeleteCourse;

public sealed record DeleteCourseCommand(Guid Id)
    : IRequest<Either<CourseException, LanguageExt.Unit>>;
