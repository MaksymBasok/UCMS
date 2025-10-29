namespace UCMS.Domain.Schedules;

public sealed class CourseSchedule
{
    public Guid Id { get; }
    public Guid CourseId { get; }
    public string Topic { get; private set; }
    public CourseScheduleFrequency Frequency { get; private set; }
    public DateTime NextSessionDate { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private CourseSchedule(Guid id, Guid courseId, string topic, CourseScheduleFrequency frequency, DateTime nextSessionDate, bool isActive, DateTime createdAt, DateTime? updatedAt)
    {
        Id = id;
        CourseId = courseId;
        Topic = topic;
        Frequency = frequency;
        NextSessionDate = nextSessionDate;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static CourseSchedule New(Guid id, Guid courseId, string topic, CourseScheduleFrequency frequency, DateTime nextSessionDate)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id");
        if (courseId == Guid.Empty) throw new ArgumentException("CourseId");
        if (string.IsNullOrWhiteSpace(topic) || topic.Length > 100) throw new ArgumentException("Topic");
        if (nextSessionDate <= DateTime.UtcNow) throw new ArgumentException("NextSessionDate");

        return new CourseSchedule(
            id,
            courseId,
            topic.Trim(),
            frequency,
            nextSessionDate,
            true,
            DateTime.UtcNow,
            null);
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
