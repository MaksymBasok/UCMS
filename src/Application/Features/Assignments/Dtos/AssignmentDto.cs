namespace UCMS.Application.Features.Assignments.Dtos;
using UCMS.Domain.Assignments;

public sealed record AssignmentDto
{
    public Guid Id { get; init; }
    public Guid CourseId { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public DateTime DueDate { get; init; }
    public string Status { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public static AssignmentDto From(Assignment a) => new()
    {
        Id = a.Id,
        CourseId = a.CourseId,
        Title = a.Title,
        Description = a.Description,
        DueDate = a.DueDate,
        Status = a.Status.ToString(),
        CreatedAt = a.CreatedAt,
        UpdatedAt = a.UpdatedAt
    };
}
