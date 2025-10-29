using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCMS.Domain.Assignments;
using UCMS.Domain.Courses;

namespace UCMS.Infrastructure.Persistence.Configurations;
public sealed class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> b)
    {
        b.ToTable("assignments");
        b.HasKey(x => x.Id);
        b.Property(x => x.Title).IsRequired().HasMaxLength(200);
        b.Property(x => x.Description).IsRequired().HasMaxLength(1000);
        b.Property(x => x.DueDate).IsRequired();
        b.Property(x => x.Status).IsRequired();
        b.Property(x => x.CreatedAt).IsRequired();

        b.HasOne<Course>()
         .WithMany()
         .HasForeignKey(x => x.CourseId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}
