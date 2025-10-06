using FluentValidation;
using UCMS.Application.Features.Courses.Commands.UpdateCourse;

namespace UCMS.Application.Features.Courses.Validators;

public sealed class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Credits).InclusiveBetween(1, 60);
    }
}
