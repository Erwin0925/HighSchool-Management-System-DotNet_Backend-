using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HighSchool_Management_System.Model
{
    public class SchoolClass
    {
        [Key]
        public int SchoolClassId { get; set; } // Auto-incremented primary key

        [Required]
        [MaxLength(50)]
        public string ClassName { get; set; }

        // Foreign Key for Form Teacher
        [ForeignKey("Teacher")]
        public int? FormTeacherId { get; set; } // Nullable to handle no assigned teacher initially

        // Navigation Property
        public virtual Teacher? FormTeacher { get; set; }

        // Navigation Property
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
