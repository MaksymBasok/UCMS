namespace UCMS.Application.Features.Students.Exceptions;

public abstract class StudentException(Guid studentId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid StudentId { get; } = studentId;
}

public sealed class StudentAlreadyExistsException(Guid studentId, string studentNumber)
    : StudentException(studentId, $"Student with number '{studentNumber}' already exists.")
{
    public string StudentNumber { get; } = studentNumber;
}

public sealed class StudentNotFoundException(Guid studentId)
    : StudentException(studentId, $"Student '{studentId}' was not found.");

public sealed class StudentValidationException(Guid studentId, string message)
    : StudentException(studentId, message);

public sealed class StudentUnexpectedException(Guid studentId, Exception innerException)
    : StudentException(studentId, "Unexpected error occurred while processing the student.", innerException);
