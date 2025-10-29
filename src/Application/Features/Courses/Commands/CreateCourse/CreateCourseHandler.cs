using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Application.Features.Courses.Exceptions;
using UCMS.Domain.Courses;

namespace UCMS.Application.Features.Courses.Commands.CreateCourse;

public sealed class CreateCourseHandler
    : IRequestHandler<CreateCourseCommand, Either<CourseException, CourseDto>>
{
    private readonly ICourseRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateCourseHandler(ICourseRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Either<CourseException, CourseDto>> Handle(
        CreateCourseCommand request,
        CancellationToken ct)
    {
        try
        {
            var normalizedCode = request.Code.Trim();
            var existing = await _repo.GetByCodeAsync(normalizedCode, ct);
            if (existing is not null)
            {
                return new CourseAlreadyExistsException(existing.Id, existing.Code);
            }

            var course = Course.New(Guid.NewGuid(), normalizedCode, request.Title, request.Description, request.Credits);
            await _repo.AddAsync(course, ct);
            await _uow.SaveChangesAsync(ct);

            return CourseDto.From(course);
        }
        catch (ArgumentException ex)
        {
            return new CourseValidationException(Guid.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            return new CourseUnexpectedException(Guid.Empty, ex);
        }
    }
}
