using FluentValidation;

namespace UCMS.Application.Features.Assignments.Commands.UpdateAssignment;

public sealed class UpdateAssignmentValidator : AbstractValidator<UpdateAssignmentCommand>
{
    public UpdateAssignmentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.DueDate)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Due date must be in the future.");
    }
}
