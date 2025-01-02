using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HighSchool_Management_System.Model
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; } // Auto-incremented primary key

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; } // Optional

        [MaxLength(10)]
        public string? Gender { get; set; } // Optional

        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; } // Optional

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; } // Optional

        [ForeignKey("SchoolClass")]
        public int? SchoolClassId { get; set; } // Update Foreign Key Name

        // Navigation Property
        public virtual SchoolClass? SchoolClass { get; set; }
    }
}
