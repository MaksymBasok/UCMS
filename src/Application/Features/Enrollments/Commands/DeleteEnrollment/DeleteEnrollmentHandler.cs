using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Enrollments.Exceptions;

namespace UCMS.Application.Features.Enrollments.Commands.DeleteEnrollment;

public sealed class DeleteEnrollmentHandler
    : IRequestHandler<DeleteEnrollmentCommand, Either<EnrollmentException, LanguageExt.Unit>>
{
    private readonly IEnrollmentRepository _repo;

    public DeleteEnrollmentHandler(IEnrollmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Either<EnrollmentException, LanguageExt.Unit>> Handle(
        DeleteEnrollmentCommand request,
        CancellationToken ct)
    {
        try
        {
            var enrollment = await _repo.GetByIdAsync(request.Id, ct);
            if (enrollment is null)
            {
                return new EnrollmentNotFoundException(request.Id);
            }

            await _repo.RemoveAsync(enrollment, ct);

            return LanguageExt.Unit.Default;
        }
        catch (Exception ex)
        {
            return new EnrollmentUnexpectedException(request.Id, ex);
        }
    }
}
