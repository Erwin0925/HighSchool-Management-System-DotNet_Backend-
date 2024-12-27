using HighSchool_Management_System.Data;
using HighSchool_Management_System.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HighSchool_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolClassController : ControllerBase
    {
        private readonly DataContext _context;

        public SchoolClassController(DataContext context)
        {
            _context = context;
        }

        // GET: api/SchoolClass
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolClass>>> GetSchoolClasses()
        {
            var schoolClasses = await _context.SchoolClasses
                .Include(sc => sc.FormTeacher) // Eagerly load FormTeacher
                .Include(sc => sc.Students) // Eagerly load Students
                .ToListAsync();

            return Ok(schoolClasses);
        }


    }
}
