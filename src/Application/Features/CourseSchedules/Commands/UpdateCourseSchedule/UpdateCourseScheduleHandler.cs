using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Application.Features.CourseSchedules.Exceptions;

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
        try
        {
            var schedule = await _repo.GetByIdAsync(request.Id, ct)
                ?? throw new CourseScheduleNotFoundException(request.Id);

            schedule.UpdateSchedule(request.Topic, request.Frequency, request.NextSessionDate);

            await _repo.UpdateAsync(schedule, ct);

            return CourseScheduleDto.From(schedule);
        }
        catch (ArgumentException ex)
        {
            throw new CourseScheduleValidationException(request.Id, ex.Message);
        }
        catch (CourseScheduleException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new CourseScheduleUnexpectedException(request.Id, ex);
        }
    }
}
