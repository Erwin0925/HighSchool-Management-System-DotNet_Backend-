using HighSchool_Management_System.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HighSchool_Management_System.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.TeacherId);

            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(t => t.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasOne(t => t.FormClass)
                   .WithOne(c => c.FormTeacher)
                   .HasForeignKey<SchoolClass>(c => c.FormTeacherId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
