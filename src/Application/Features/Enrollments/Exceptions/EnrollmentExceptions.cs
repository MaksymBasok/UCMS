namespace UCMS.Application.Features.Enrollments.Exceptions;

public abstract class EnrollmentException(Guid enrollmentId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid EnrollmentId { get; } = enrollmentId;
}

public sealed class EnrollmentAlreadyExistsException(Guid enrollmentId)
    : EnrollmentException(enrollmentId, "Student is already enrolled to this course.");

public sealed class EnrollmentNotFoundException(Guid enrollmentId)
    : EnrollmentException(enrollmentId, $"Enrollment '{enrollmentId}' was not found.");

public sealed class EnrollmentValidationException(Guid enrollmentId, string message)
    : EnrollmentException(enrollmentId, message);

public sealed class EnrollmentUnexpectedException(Guid enrollmentId, Exception innerException)
    : EnrollmentException(enrollmentId, "Unexpected error occurred while processing the enrollment.", innerException);
