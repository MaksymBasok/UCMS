using UCMS.Domain.Submissions;

namespace Tests.Data.Submissions;

public static class SubmissionData
{
    public static Submission OpenSubmission(Guid assignmentId, Guid studentId)
        => Submission.New(
            new Guid("f1f2f3f4-f5f6-4f7f-8f9f-fa0fb1fc2fd3"),
            assignmentId,
            studentId,
            "https://example.com/submissions/initial",
            DateTime.UtcNow);

    public static Submission CompletedSubmission(Guid assignmentId, Guid studentId)
    {
        var submission = Submission.New(
            new Guid("0a1b2c3d-4e5f-6a7b-8c9d-0e1f2a3b4c5d"),
            assignmentId,
            studentId,
            "https://example.com/submissions/completed",
            DateTime.UtcNow.AddMinutes(-10));
        submission.StartReview();
        submission.Complete("Reviewed and scored", 95);
        return submission;
    }
}
