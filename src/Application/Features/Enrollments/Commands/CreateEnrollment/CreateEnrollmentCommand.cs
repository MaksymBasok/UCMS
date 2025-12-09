using LanguageExt;
using MediatR;
using UCMS.Application.Features.Enrollments.Dtos;
using UCMS.Application.Features.Enrollments.Exceptions;

namespace UCMS.Application.Features.Enrollments.Commands.CreateEnrollment;

public sealed record CreateEnrollmentCommand(Guid StudentId, Guid CourseId)
    : IRequest<Either<EnrollmentException, EnrollmentDto>>;
