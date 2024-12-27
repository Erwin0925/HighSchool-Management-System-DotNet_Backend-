using HighSchool_Management_System.Data;
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
                .Include(s => s.SchoolClass) // Include SchoolClass
                    .ThenInclude(sc => sc.FormTeacher) // Include FormTeacher in SchoolClass
                .ToListAsync();

            return Ok(students);
        }

    }
}
