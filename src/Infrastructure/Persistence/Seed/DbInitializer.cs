using UCMS.Domain.Courses;
using UCMS.Domain.Schedules;
using UCMS.Domain.Submissions;

namespace UCMS.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext db, CancellationToken ct = default)
    {
        if (!db.Courses.Any())
        {
            var c1 = Course.New(Guid.NewGuid(), "ALGO-101", "Algorithms", "Intro to algorithms", 6);
            var c2 = Course.New(Guid.NewGuid(), "DB-201", "Databases", "Relational databases and SQL", 5);
            var c3 = Course.New(Guid.NewGuid(), "OS-301", "Operating Systems", "Processes, memory, IO", 6);
            db.Courses.AddRange(c1, c2, c3);

            var s1 = CourseSchedule.New(Guid.NewGuid(), c1.Id, "Monthly revision", CourseScheduleFrequency.Monthly, DateTime.UtcNow.AddDays(7));
            var s2 = CourseSchedule.New(Guid.NewGuid(), c2.Id, "Quarterly practice", CourseScheduleFrequency.Quarterly, DateTime.UtcNow.AddDays(30));
            var s3 = CourseSchedule.New(Guid.NewGuid(), c3.Id, "Annual recap", CourseScheduleFrequency.Annually, DateTime.UtcNow.AddDays(120));
            db.CourseSchedules.AddRange(s1, s2, s3);

            var subOpen = Submission.New(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "https://files/sub-open.pdf", DateTime.UtcNow);
            var subProg = Submission.New(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "https://files/sub-inprogress.pdf", DateTime.UtcNow);
            subProg.StartReview();
            var subDone = Submission.New(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "https://files/sub-completed.pdf", DateTime.UtcNow);
            subDone.StartReview(); subDone.Complete("Looks good", 95m);

            db.Submissions.AddRange(subOpen, subProg, subDone);
        }

        await db.SaveChangesAsync(ct);
    }
}
