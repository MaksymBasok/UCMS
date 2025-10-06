using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.CourseSchedules.Dtos;

namespace UCMS.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule;
public sealed class UpdateCourseScheduleHandler : IRequestHandler<UpdateCourseScheduleCommand, CourseScheduleDto>
{
    private readonly ICourseScheduleRepository _repo; private readonly IUnitOfWork _uow;
    public UpdateCourseScheduleHandler(ICourseScheduleRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
    public async Task<CourseScheduleDto> Handle(UpdateCourseScheduleCommand r, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Schedule not found");
        s.UpdateSchedule(r.Topic, r.Frequency, r.NextSessionDate);
        await _uow.SaveChangesAsync(ct); return CourseScheduleDto.From(s);
    }
}
