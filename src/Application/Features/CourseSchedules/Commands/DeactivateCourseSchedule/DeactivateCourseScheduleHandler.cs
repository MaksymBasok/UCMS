using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;

namespace UCMS.Application.Features.CourseSchedules.Commands.DeactivateCourseSchedule;
public sealed class DeactivateCourseScheduleHandler : IRequestHandler<DeactivateCourseScheduleCommand, Unit>
{
    private readonly ICourseScheduleRepository _repo; private readonly IUnitOfWork _uow;
    public DeactivateCourseScheduleHandler(ICourseScheduleRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
    public async Task<Unit> Handle(DeactivateCourseScheduleCommand r, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Schedule not found");
        s.Deactivate(); await _uow.SaveChangesAsync(ct); return Unit.Value;
    }
}
