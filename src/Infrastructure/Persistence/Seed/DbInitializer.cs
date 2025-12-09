using Microsoft.EntityFrameworkCore;
using UCMS.Domain.Courses;
using UCMS.Domain.Schedules;
using UCMS.Domain.Assignments;
using UCMS.Domain.Submissions;

namespace UCMS.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext db, CancellationToken ct = default)
    {
        // 1) Courses
        if (!await db.Courses.AnyAsync(ct))
        {
            var c1 = Course.New(Guid.NewGuid(), "ALGO-101", "Algorithms", "Intro to algorithms", 6);
            var c2 = Course.New(Guid.NewGuid(), "DB-201", "Databases", "Relational databases and SQL", 5);
            var c3 = Course.New(Guid.NewGuid(), "OS-301", "Operating Systems", "Processes, memory, IO", 6);
            db.Courses.AddRange(c1, c2, c3);

            // 2) Course schedules (прив’язані до курсів)
            var s1 = CourseSchedule.New(Guid.NewGuid(), c1.Id, "Monthly revision", CourseScheduleFrequency.Monthly, DateTime.UtcNow.AddDays(7));
            var s2 = CourseSchedule.New(Guid.NewGuid(), c2.Id, "Quarterly practice", CourseScheduleFrequency.Quarterly, DateTime.UtcNow.AddDays(30));
            var s3 = CourseSchedule.New(Guid.NewGuid(), c3.Id, "Annual recap", CourseScheduleFrequency.Annually, DateTime.UtcNow.AddDays(120));
            db.CourseSchedules.AddRange(s1, s2, s3);

            // 3) Assignments (прив’язані до курсів)
            var a1 = Assignment.New(Guid.NewGuid(), c1.Id, "HW1: Sorting", "Implement quicksort", DateTime.UtcNow.AddDays(10));
            var a2 = Assignment.New(Guid.NewGuid(), c2.Id, "HW1: SQL Joins", "Write complex joins", DateTime.UtcNow.AddDays(14));
            var a3 = Assignment.New(Guid.NewGuid(), c3.Id, "HW1: Processes", "Process scheduling", DateTime.UtcNow.AddDays(21));
            a1.Publish(); a2.Publish(); a3.Publish();
            db.Assignments.AddRange(a1, a2, a3);
        }

        await db.SaveChangesAsync(ct);

        // 4) Submissions — лише якщо є і студент, і асайнмент
        if (!await db.Submissions.AnyAsync(ct))
        {
            var studentId = await db.Students.AsNoTracking().Select(x => x.Id).FirstOrDefaultAsync(ct);
            var assignmentId = await db.Assignments.AsNoTracking().Select(x => x.Id).FirstOrDefaultAsync(ct);

            if (studentId != Guid.Empty && assignmentId != Guid.Empty)
            {
                // Створюємо один submission (початковий стан)
                var submission = Submission.New(
                    Guid.NewGuid(),
                    assignmentId,
                    studentId,
                    "https://files/sub-open.pdf",
                    DateTime.UtcNow
                );

                db.Submissions.Add(submission);
                await db.SaveChangesAsync(ct);

                // Перехід у стан "InProgress"
                submission.StartReview();
                await db.SaveChangesAsync(ct);

                // Завершення роботи
                submission.Complete("Looks good", 95m);
                await db.SaveChangesAsync(ct);
            }
        }
    }
}
