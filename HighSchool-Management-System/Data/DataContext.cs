using HighSchool_Management_System.Model;
using Microsoft.EntityFrameworkCore;

namespace HighSchool_Management_System.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations using Fluent API
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}
