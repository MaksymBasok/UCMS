namespace UCMS.Domain.Schedules;

public sealed class CourseSchedule
{
    public Guid Id { get; private set; }
    public Guid CourseId { get; private set; }
    public string Topic { get; private set; } = default!;
    public CourseScheduleFrequency Frequency { get; private set; }
    public DateTime NextSessionDate { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private CourseSchedule() { }

    public static CourseSchedule New(Guid id, Guid courseId, string topic, CourseScheduleFrequency frequency, DateTime nextSessionDate)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (courseId == Guid.Empty) throw new ArgumentException("CourseId");
        if (string.IsNullOrWhiteSpace(topic) || topic.Length > 100) throw new ArgumentException("Topic");
        if (nextSessionDate <= DateTime.UtcNow) throw new ArgumentException("NextSessionDate");

        return new CourseSchedule
        {
            Id = id,
            CourseId = courseId,
            Topic = topic.Trim(),
            Frequency = frequency,
            NextSessionDate = nextSessionDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateSchedule(string topic, CourseScheduleFrequency frequency, DateTime nextSessionDate)
    {
        if (string.IsNullOrWhiteSpace(topic) || topic.Length > 100) throw new ArgumentException("Topic");
        if (nextSessionDate <= DateTime.UtcNow) throw new ArgumentException("NextSessionDate");

        Topic = topic.Trim();
        Frequency = frequency;
        NextSessionDate = nextSessionDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum CourseScheduleFrequency { Daily, Weekly, Monthly, Quarterly, Annually }
