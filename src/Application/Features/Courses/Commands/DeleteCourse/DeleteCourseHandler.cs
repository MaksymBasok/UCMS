using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Courses.Exceptions;

namespace UCMS.Application.Features.Courses.Commands.DeleteCourse;

public sealed class DeleteCourseHandler
    : IRequestHandler<DeleteCourseCommand, Either<CourseException, LanguageExt.Unit>>
{
    private readonly ICourseRepository _repo;

    public DeleteCourseHandler(ICourseRepository repo)
    {
        _repo = repo;
    }

    public async Task<Either<CourseException, LanguageExt.Unit>> Handle(
        DeleteCourseCommand request,
        CancellationToken ct)
    {
        var course = await _repo.GetByIdAsync(request.Id, ct);
        if (course is null)
        {
            return new CourseNotFoundException(request.Id);
        }

        await _repo.RemoveAsync(course, ct);

        return LanguageExt.Unit.Default;
    }
}
