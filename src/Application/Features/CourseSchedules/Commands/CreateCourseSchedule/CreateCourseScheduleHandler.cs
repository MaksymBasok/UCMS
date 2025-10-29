using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Abstractions;
using UCMS.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Domain.Schedules;

public sealed class CreateCourseScheduleHandler
    : IRequestHandler<CreateCourseScheduleCommand, CourseScheduleDto>
{
    private readonly ICourseScheduleRepository _repo;
    private readonly ICourseRepository _courseRepo;
    private readonly IUnitOfWork _uow;

    public CreateCourseScheduleHandler(
        ICourseScheduleRepository repo,
        ICourseRepository courseRepo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _courseRepo = courseRepo;
        _uow = uow;
    }

    public async Task<CourseScheduleDto> Handle(CreateCourseScheduleCommand r, CancellationToken ct)
    {
        if (await _courseRepo.GetByIdAsync(r.CourseId, ct) is null)
            throw new KeyNotFoundException($"Course {r.CourseId} not found");

        var schedule = CourseSchedule.New(Guid.NewGuid(), r.CourseId, r.Topic,
                                         r.Frequency, r.NextSessionDate);
        await _repo.AddAsync(schedule, ct);
        await _uow.SaveChangesAsync(ct);
        return CourseScheduleDto.From(schedule);
    }
}
