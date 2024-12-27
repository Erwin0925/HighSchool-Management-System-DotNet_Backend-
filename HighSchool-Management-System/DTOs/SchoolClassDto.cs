namespace HighSchool_Management_System.DTOs
{
    public class SchoolClassDto
    {
        public int SchoolClassId { get; set; }
        public string ClassName { get; set; }
        public string FormTeacherName { get; set; }
        public List<string> StudentNames { get; set; } // Simplified student list
    }
}
