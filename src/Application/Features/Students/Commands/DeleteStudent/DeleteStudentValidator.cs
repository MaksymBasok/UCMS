using FluentValidation;

namespace UCMS.Application.Features.Students.Commands.DeleteStudent;

public sealed class DeleteStudentValidator : AbstractValidator<DeleteStudentCommand>
{
    public DeleteStudentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
