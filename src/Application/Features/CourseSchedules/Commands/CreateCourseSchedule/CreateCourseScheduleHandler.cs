using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Domain.Schedules;

namespace UCMS.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;
public sealed class CreateCourseScheduleHandler : IRequestHandler<CreateCourseScheduleCommand, CourseScheduleDto>
{
    private readonly ICourseScheduleRepository _repo; private readonly IUnitOfWork _uow;
    public CreateCourseScheduleHandler(ICourseScheduleRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
    public async Task<CourseScheduleDto> Handle(CreateCourseScheduleCommand r, CancellationToken ct)
    {
        var s = CourseSchedule.New(Guid.NewGuid(), r.CourseId, r.Topic, r.Frequency, r.NextSessionDate);
        await _repo.AddAsync(s, ct); await _uow.SaveChangesAsync(ct); return CourseScheduleDto.From(s);
    }
}
