namespace UCMS.Application.Features.Submissions.Exceptions;

public abstract class SubmissionException(Guid submissionId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid SubmissionId { get; } = submissionId;
}

public sealed class SubmissionNotFoundException(Guid submissionId)
    : SubmissionException(submissionId, $"Submission '{submissionId}' was not found.");

public sealed class SubmissionAlreadyExistsException(Guid submissionId)
    : SubmissionException(submissionId, $"Submission '{submissionId}' already exists.");

public sealed class SubmissionValidationException(Guid submissionId, string message)
    : SubmissionException(submissionId, message);

public sealed class SubmissionUnexpectedException(Guid submissionId, Exception innerException)
    : SubmissionException(
        submissionId,
        "Unexpected error occurred while processing the submission.",
        innerException);
