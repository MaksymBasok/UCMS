namespace UCMS.Domain.Submissions;

public enum SubmissionStatus { Open, InProgress, Completed, Cancelled }

public sealed class Submission
{
    public Guid Id { get; }
    public Guid AssignmentId { get; }
    public Guid StudentId { get; }
    public DateTime SubmittedAt { get; private set; }
    public string ContentUrl { get; private set; }
    public SubmissionStatus Status { get; private set; }
    public decimal? Grade { get; private set; }
    public string? CompletionNotes { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private Submission(Guid id, Guid assignmentId, Guid studentId, DateTime submittedAt, string contentUrl, SubmissionStatus status, decimal? grade, string? completionNotes, DateTime createdAt, DateTime? updatedAt)
    {
        Id = id;
        AssignmentId = assignmentId;
        StudentId = studentId;
        SubmittedAt = submittedAt;
        ContentUrl = contentUrl;
        Status = status;
        Grade = grade;
        CompletionNotes = completionNotes;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Submission New(Guid id, Guid assignmentId, Guid studentId, string contentUrl, DateTime submittedAtUtc)
    {
        if (id == Guid.Empty || assignmentId == Guid.Empty || studentId == Guid.Empty) throw new ArgumentException("Ids");
        if (string.IsNullOrWhiteSpace(contentUrl) || contentUrl.Length > 500) throw new ArgumentException("ContentUrl");

        return new Submission(
            id,
            assignmentId,
            studentId,
            submittedAtUtc,
            contentUrl.Trim(),
            SubmissionStatus.Open,
            null,
            null,
            DateTime.UtcNow,
            null);
    }

    public void UpdateContent(string contentUrl)
    {
        if (Status is SubmissionStatus.Completed or SubmissionStatus.Cancelled) throw new InvalidOperationException("Locked");
        if (string.IsNullOrWhiteSpace(contentUrl) || contentUrl.Length > 500) throw new ArgumentException("ContentUrl");
        ContentUrl = contentUrl.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void StartReview()
    {
        if (Status != SubmissionStatus.Open) throw new InvalidOperationException("Only from Open");
        Status = SubmissionStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete(string completionNotes, decimal grade)
    {
        if (Status != SubmissionStatus.InProgress) throw new InvalidOperationException("Only from InProgress");
        if (string.IsNullOrWhiteSpace(completionNotes) || completionNotes.Length > 1000) throw new ArgumentException("Notes");
        if (grade is < 0 or > 100) throw new ArgumentException("Grade");
        Status = SubmissionStatus.Completed;
        CompletionNotes = completionNotes.Trim();
        Grade = grade;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == SubmissionStatus.Completed) throw new InvalidOperationException("Already completed");
        Status = SubmissionStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}
