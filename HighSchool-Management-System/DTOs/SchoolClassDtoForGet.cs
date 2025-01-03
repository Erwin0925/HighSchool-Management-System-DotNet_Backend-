namespace HighSchool_Management_System.DTOs
{
    public class SchoolClassDtoForGet
    {
        public int SchoolClassId { get; set; }
        public string ClassName { get; set; }
        public string FormTeacherName { get; set; }
        public List<string> StudentNames { get; set; } // Simplified student list
    }
}
