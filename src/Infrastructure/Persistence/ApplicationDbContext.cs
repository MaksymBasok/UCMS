using Microsoft.EntityFrameworkCore;
using UCMS.Domain.Students;
using UCMS.Domain.Courses;
using UCMS.Domain.Schedules;
using UCMS.Domain.Submissions;
using UCMS.Domain.Assignments;
using UCMS.Domain.Enrollments;

namespace UCMS.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseSchedule> CourseSchedules => Set<CourseSchedule>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
