using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.CourseSchedules.Dtos;

namespace UCMS.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule;

public sealed class UpdateCourseScheduleHandler : IRequestHandler<UpdateCourseScheduleCommand, CourseScheduleDto>
{
    private readonly ICourseScheduleRepository _repo;

    public UpdateCourseScheduleHandler(ICourseScheduleRepository repo)
    {
        _repo = repo;
    }

    public async Task<CourseScheduleDto> Handle(UpdateCourseScheduleCommand request, CancellationToken ct)
    {
        var schedule = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Schedule not found");

        schedule.UpdateDates(request.StartDate, request.EndDate);

        await _repo.UpdateAsync(schedule, ct);

        return CourseScheduleDto.From(schedule);
    }
}
