using UCMS.Domain.Submissions;

namespace UCMS.Application.Features.Submissions.Dtos;
public sealed record SubmissionDto(Guid Id, Guid AssignmentId, Guid StudentId, string ContentUrl, SubmissionStatus Status, DateTime SubmittedAt, decimal? Grade, string? CompletionNotes)
{
    public static SubmissionDto From(Submission s) => new(s.Id, s.AssignmentId, s.StudentId, s.ContentUrl, s.Status, s.SubmittedAt, s.Grade, s.CompletionNotes);
}
