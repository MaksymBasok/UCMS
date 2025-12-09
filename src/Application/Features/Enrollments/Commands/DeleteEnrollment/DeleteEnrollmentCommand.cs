using LanguageExt;
using MediatR;
using UCMS.Application.Features.Enrollments.Exceptions;

namespace UCMS.Application.Features.Enrollments.Commands.DeleteEnrollment;

public sealed record DeleteEnrollmentCommand(Guid Id) : IRequest<Either<EnrollmentException, LanguageExt.Unit>>;
