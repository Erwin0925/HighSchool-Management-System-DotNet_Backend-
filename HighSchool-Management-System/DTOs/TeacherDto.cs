﻿namespace HighSchool_Management_System.DTOs
{
    public class TeacherDto
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string? FormClassName { get; set; } // Simplified relationship
    }
}
