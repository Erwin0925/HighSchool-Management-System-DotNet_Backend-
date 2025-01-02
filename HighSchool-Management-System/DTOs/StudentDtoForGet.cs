namespace HighSchool_Management_System.DTOs
{
    public class StudentDtoForGet
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int SchoolClassId { get; set; }
        public string SchoolClassName { get; set; } // Flattened SchoolClass relationship
        public string FormTeacherName { get; set; } // Flattened FormTeacher relationship
    }
}
