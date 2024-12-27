using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HighSchool_Management_System.Model
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; } // Auto-incremented primary key

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; } // Optional

        [MaxLength(255)]
        public string? Subject { get; set; } // Optional

        // Navigation Property
        public virtual SchoolClass? FormClass { get; set; } // Update Navigation Property Name
    }
}
