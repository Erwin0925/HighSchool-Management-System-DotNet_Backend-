using HighSchool_Management_System.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HighSchool_Management_System.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.StudentId);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(s => s.Email)
                   .HasMaxLength(255);

            builder.HasOne(s => s.SchoolClass)
                   .WithMany(c => c.Students)
                   .HasForeignKey(s => s.SchoolClassId);
        }
    }
}
