using LanguageExt;
using MediatR;
using UCMS.Application.Features.Enrollments.Dtos;
using UCMS.Application.Features.Enrollments.Exceptions;

namespace UCMS.Application.Features.Enrollments.Commands.CompleteEnrollment;

public sealed record CompleteEnrollmentCommand(Guid Id)
    : IRequest<Either<EnrollmentException, EnrollmentDto>>;
