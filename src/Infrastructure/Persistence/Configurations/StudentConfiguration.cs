using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCMS.Domain.Students;

namespace UCMS.Infrastructure.Persistence.Configurations;

public sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> b)
    {
        b.ToTable("students");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.StudentNumber).IsRequired().HasMaxLength(20);
        b.HasIndex(x => x.StudentNumber).IsUnique();
        b.Property(x => x.FullName).IsRequired().HasMaxLength(100);
        b.Property(x => x.Email).IsRequired().HasMaxLength(200);
        b.Property(x => x.CreatedAt).IsRequired();

        // 1:1 Profile (optional)
        b.HasOne<StudentProfile>()
         .WithOne()
         .HasForeignKey<StudentProfile>(p => p.StudentId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}
