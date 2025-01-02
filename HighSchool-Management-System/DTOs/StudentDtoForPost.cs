namespace HighSchool_Management_System.DTOs
{
    public class StudentDtoForPost
    {
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int SchoolClassId { get; set; }
    }
}
