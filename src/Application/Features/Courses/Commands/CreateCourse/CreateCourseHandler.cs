using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Domain.Courses;

namespace UCMS.Application.Features.Courses.Commands.CreateCourse;

public sealed class CreateCourseHandler : IRequestHandler<CreateCourseCommand, CourseDto>
{
    private readonly ICourseRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateCourseHandler(ICourseRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<CourseDto> Handle(CreateCourseCommand r, CancellationToken ct)
    {
        var course = Course.New(Guid.NewGuid(), r.Code, r.Title, r.Description, r.Credits);
        await _repo.AddAsync(course, ct);
        await _uow.SaveChangesAsync(ct);
        return CourseDto.From(course);
    }
}
