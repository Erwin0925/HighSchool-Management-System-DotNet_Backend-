using HighSchool_Management_System.Data;
using HighSchool_Management_System.DTOs;
using HighSchool_Management_System.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HighSchool_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students
                        .Include(s => s.SchoolClass)
                            .ThenInclude(sc => sc.FormTeacher)
                        .Select(s => new StudentDto
                        {
                            StudentId = s.StudentId,
                            Name = s.Name,
                            Gender = s.Gender,
                            Email = s.Email,
                            Phone = s.Phone,
                            ClassName = s.SchoolClass.ClassName,
                            FormTeacherName = s.SchoolClass.FormTeacher != null ? s.SchoolClass.FormTeacher.Name : null // Null check
                        })
                        .ToListAsync();

            return Ok(students);
        }

    }
}
