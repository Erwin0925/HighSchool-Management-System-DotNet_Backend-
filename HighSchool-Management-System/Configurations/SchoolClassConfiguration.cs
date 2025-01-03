using HighSchool_Management_System.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HighSchool_Management_System.Configurations
{
    public class SchoolClassConfiguration : IEntityTypeConfiguration<SchoolClass>
    {
        public void Configure(EntityTypeBuilder<SchoolClass> builder)
        {
            builder.HasKey(c => c.SchoolClassId);

            builder.Property(c => c.ClassName)
                   .HasMaxLength(50);

            builder.HasIndex(c => c.ClassName)
                   .IsUnique();

            builder.HasMany(c => c.Students)
                   .WithOne(s => s.SchoolClass)
                   .HasForeignKey(s => s.SchoolClassId);
        }
    }
}
