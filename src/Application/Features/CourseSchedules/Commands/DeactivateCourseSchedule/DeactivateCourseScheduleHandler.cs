using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.CourseSchedules.Exceptions;

namespace UCMS.Application.Features.CourseSchedules.Commands.DeactivateCourseSchedule;

public sealed class DeactivateCourseScheduleHandler : IRequestHandler<DeactivateCourseScheduleCommand, MediatR.Unit>
{
    private readonly ICourseScheduleRepository _repo;

    public DeactivateCourseScheduleHandler(ICourseScheduleRepository repo)
    {
        _repo = repo;
    }

    public async Task<MediatR.Unit> Handle(DeactivateCourseScheduleCommand request, CancellationToken ct)
    {
        var schedule = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new CourseScheduleNotFoundException(request.Id);

        schedule.Deactivate();
        await _repo.UpdateAsync(schedule, ct);

        return MediatR.Unit.Value;
    }
}
