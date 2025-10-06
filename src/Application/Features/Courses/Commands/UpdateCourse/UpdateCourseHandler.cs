using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Courses.Commands.UpdateCourse;
using UCMS.Application.Features.Courses.Dtos;

namespace UCMS.Application.Features.Courses.Commands.UpdateCourse;

public sealed class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, CourseDto>
{
    private readonly ICourseRepository _repo; private readonly IUnitOfWork _uow;
    public UpdateCourseHandler(ICourseRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<CourseDto> Handle(UpdateCourseCommand r, CancellationToken ct)
    {
        var course = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Course not found");
        course.UpdateDetails(r.Title, r.Description, r.Credits);
        await _uow.SaveChangesAsync(ct);
        return CourseDto.From(course);
    }
}
