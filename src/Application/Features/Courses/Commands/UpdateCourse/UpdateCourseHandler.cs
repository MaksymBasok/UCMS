using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Application.Features.Courses.Exceptions;

namespace UCMS.Application.Features.Courses.Commands.UpdateCourse;

public sealed class UpdateCourseHandler
    : IRequestHandler<UpdateCourseCommand, Either<CourseException, CourseDto>>
{
    private readonly ICourseRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateCourseHandler(ICourseRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Either<CourseException, CourseDto>> Handle(
        UpdateCourseCommand request,
        CancellationToken ct)
    {
        try
        {
            var course = await _repo.GetByIdAsync(request.Id, ct);
            if (course is null)
            {
                return new CourseNotFoundException(request.Id);
            }

            course.UpdateDetails(request.Title, request.Description, request.Credits);
            await _uow.SaveChangesAsync(ct);

            return CourseDto.From(course);
        }
        catch (ArgumentException ex)
        {
            return new CourseValidationException(request.Id, ex.Message);
        }
        catch (Exception ex)
        {
            return new CourseUnexpectedException(request.Id, ex);
        }
    }
}
