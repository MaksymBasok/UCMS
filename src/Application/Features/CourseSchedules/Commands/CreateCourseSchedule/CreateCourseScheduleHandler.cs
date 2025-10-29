using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Application.Features.CourseSchedules.Exceptions;
using UCMS.Domain.Schedules;

namespace UCMS.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;

public sealed class CreateCourseScheduleHandler
    : IRequestHandler<CreateCourseScheduleCommand, Either<CourseScheduleException, CourseScheduleDto>>
{
    private readonly ICourseScheduleRepository _repo;

    public CreateCourseScheduleHandler(
        ICourseScheduleRepository repo)
    {
        _repo = repo;
    }

    public async Task<Either<CourseScheduleException, CourseScheduleDto>> Handle(
        CreateCourseScheduleCommand request,
        CancellationToken ct)
    {
        try
        {
            var schedule = CourseSchedule.New(
                Guid.NewGuid(),
                request.CourseId,
                request.StartDate,
                request.EndDate,
                request.IsActive);

            await _repo.AddAsync(schedule, ct);

            return CourseScheduleDto.From(schedule);
        }
        catch (ArgumentException ex)
        {
            return new CourseScheduleValidationException(Guid.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            return new CourseScheduleUnexpectedException(Guid.Empty, ex);
        }
    }
}
