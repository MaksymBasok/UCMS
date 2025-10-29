using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCMS.Domain.Submissions;

namespace UCMS.Infrastructure.Persistence.Configurations;

public sealed class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> b)
    {
        b.ToTable("submissions");
        b.HasKey(x => x.Id);
        b.Property(x => x.ContentUrl).IsRequired().HasMaxLength(500);
        b.Property(x => x.Status).IsRequired();
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.CompletionNotes).HasMaxLength(1000);
        b.HasIndex(x => new { x.AssignmentId, x.StudentId }).IsUnique();

        
        b.HasOne<UCMS.Domain.Students.Student>()
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        
        b.HasOne<UCMS.Domain.Assignments.Assignment>()
            .WithMany()
            .HasForeignKey(x => x.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
