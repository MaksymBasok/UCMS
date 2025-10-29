namespace UCMS.Application.Features.Courses.Exceptions;

public abstract class CourseException(Guid courseId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid CourseId { get; } = courseId;
}

public sealed class CourseAlreadyExistsException(Guid courseId, string code)
    : CourseException(courseId, $"Course with code '{code}' already exists.")
{
    public string Code { get; } = code;
}

public sealed class CourseNotFoundException(Guid courseId)
    : CourseException(courseId, $"Course '{courseId}' was not found.");

public sealed class CourseValidationException(Guid courseId, string message)
    : CourseException(courseId, message);

public sealed class CourseUnexpectedException(Guid courseId, Exception innerException)
    : CourseException(courseId, "Unexpected error occurred while processing the course.", innerException);
