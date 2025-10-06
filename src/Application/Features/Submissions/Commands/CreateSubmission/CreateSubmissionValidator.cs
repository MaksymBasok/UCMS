using FluentValidation;

namespace UCMS.Application.Features.Submissions.Commands.CreateSubmission;
public sealed class CreateSubmissionValidator : AbstractValidator<CreateSubmissionCommand>
{
    public CreateSubmissionValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.ContentUrl).NotEmpty().MaximumLength(500);
    }
}
