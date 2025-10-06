using FluentValidation;

namespace UCMS.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;
public sealed class CreateCourseScheduleValidator : AbstractValidator<CreateCourseScheduleCommand>
{
    public CreateCourseScheduleValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.Topic).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NextSessionDate).Must(d => d > DateTime.UtcNow);
    }
}
