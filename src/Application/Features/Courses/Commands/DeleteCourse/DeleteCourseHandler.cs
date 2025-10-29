using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Courses.Exceptions;

namespace UCMS.Application.Features.Courses.Commands.DeleteCourse;

public sealed class DeleteCourseHandler
    : IRequestHandler<DeleteCourseCommand, Either<CourseException, LanguageExt.Unit>>
{
    private readonly ICourseRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteCourseHandler(ICourseRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Either<CourseException, LanguageExt.Unit>> Handle(
        DeleteCourseCommand request,
        CancellationToken ct)
    {
        try
        {
            var course = await _repo.GetByIdAsync(request.Id, ct);
            if (course is null)
            {
                return new CourseNotFoundException(request.Id);
            }

            await _repo.RemoveAsync(course, ct);
            await _uow.SaveChangesAsync(ct);

            return LanguageExt.Unit.Default;
        }
        catch (Exception ex)
        {
            return new CourseUnexpectedException(request.Id, ex);
        }
    }
}
