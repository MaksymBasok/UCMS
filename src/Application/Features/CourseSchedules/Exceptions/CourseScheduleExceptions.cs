namespace UCMS.Application.Features.CourseSchedules.Exceptions;

public abstract class CourseScheduleException(Guid courseScheduleId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid CourseScheduleId { get; } = courseScheduleId;
}

public sealed class CourseScheduleNotFoundException(Guid courseScheduleId)
    : CourseScheduleException(courseScheduleId, $"Course schedule '{courseScheduleId}' was not found.");

public sealed class CourseScheduleValidationException(Guid courseScheduleId, string message)
    : CourseScheduleException(courseScheduleId, message);

public sealed class CourseScheduleUnexpectedException(Guid courseScheduleId, Exception innerException)
    : CourseScheduleException(
        courseScheduleId,
        "Unexpected error occurred while processing the course schedule.",
        innerException);
