using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCMS.Domain.Students;

namespace UCMS.Infrastructure.Persistence.Configurations;

public sealed class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> b)
    {
        b.ToTable("student_profiles");
        b.HasKey(x => x.StudentId);
        b.Property(x => x.CreatedAt).IsRequired();
    }
}
