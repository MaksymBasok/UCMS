using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Enrollments.Dtos;
using UCMS.Application.Features.Enrollments.Exceptions;
using UCMS.Domain.Enrollments;

namespace UCMS.Application.Features.Enrollments.Commands.CreateEnrollment;

public sealed class CreateEnrollmentHandler
    : IRequestHandler<CreateEnrollmentCommand, Either<EnrollmentException, EnrollmentDto>>
{
    private readonly IEnrollmentRepository _repo;

    public CreateEnrollmentHandler(IEnrollmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Either<EnrollmentException, EnrollmentDto>> Handle(
        CreateEnrollmentCommand request,
        CancellationToken ct)
    {
        try
        {
            var existing = await _repo.GetByStudentAndCourseAsync(request.StudentId, request.CourseId, ct);
            if (existing is not null)
            {
                return new EnrollmentAlreadyExistsException(existing.Id);
            }

            var enrollment = Enrollment.New(Guid.NewGuid(), request.StudentId, request.CourseId);
            await _repo.AddAsync(enrollment, ct);

            return EnrollmentDto.From(enrollment);
        }
        catch (ArgumentException ex)
        {
            return new EnrollmentValidationException(Guid.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            return new EnrollmentUnexpectedException(Guid.Empty, ex);
        }
    }
}
