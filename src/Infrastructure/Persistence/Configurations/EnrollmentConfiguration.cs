using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCMS.Domain.Enrollments;

namespace UCMS.Infrastructure.Persistence.Configurations;

public sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> b)
    {
        b.ToTable("enrollments");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.Status).IsRequired();
        b.Property(x => x.EnrolledAt).IsRequired();
        b.Property(x => x.CompletedAt);
        b.Property(x => x.UpdatedAt);
        b.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();

        b.HasOne<UCMS.Domain.Students.Student>()
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne<UCMS.Domain.Courses.Course>()
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
