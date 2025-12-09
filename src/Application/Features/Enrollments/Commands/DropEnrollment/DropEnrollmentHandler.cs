using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Enrollments.Dtos;
using UCMS.Application.Features.Enrollments.Exceptions;

namespace UCMS.Application.Features.Enrollments.Commands.DropEnrollment;

public sealed class DropEnrollmentHandler
    : IRequestHandler<DropEnrollmentCommand, Either<EnrollmentException, EnrollmentDto>>
{
    private readonly IEnrollmentRepository _repo;

    public DropEnrollmentHandler(IEnrollmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Either<EnrollmentException, EnrollmentDto>> Handle(
        DropEnrollmentCommand request,
        CancellationToken ct)
    {
        try
        {
            var enrollment = await _repo.GetByIdAsync(request.Id, ct);
            if (enrollment is null)
            {
                return new EnrollmentNotFoundException(request.Id);
            }

            enrollment.Drop();
            await _repo.UpdateAsync(enrollment, ct);

            return EnrollmentDto.From(enrollment);
        }
        catch (InvalidOperationException ex)
        {
            return new EnrollmentValidationException(request.Id, ex.Message);
        }
        catch (Exception ex)
        {
            return new EnrollmentUnexpectedException(request.Id, ex);
        }
    }
}
