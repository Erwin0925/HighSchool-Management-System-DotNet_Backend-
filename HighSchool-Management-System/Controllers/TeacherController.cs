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
    public class TeacherController : ControllerBase
    {
        private readonly DataContext _context;

        public TeacherController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Teacher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.FormClass) // Eagerly load FormClass
                .Select(t => new TeacherDto
                {
                    TeacherId = t.TeacherId,
                    Name = t.Name,
                    Email = t.Email,
                    Phone = t.Phone,
                    Subject = t.Subject,
                    FormClassName = t.FormClass != null ? t.FormClass.ClassName : null // Null check
                })
                .ToListAsync();

            return Ok(teachers);
        }
    }
}
