using HighSchool_Management_System.Data;
using HighSchool_Management_System.DTOs;
using HighSchool_Management_System.Model;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPost]
        public async Task<ActionResult<TeacherDtoForPost>> CreateTeacher(TeacherDtoForPost TeacherDtoForPost)
        {
            // Map the TeacherDto to the Teacher entity
            var teacher = new Teacher
            {
                Name = TeacherDtoForPost.Name,
                Email = TeacherDtoForPost.Email,
                Phone = TeacherDtoForPost.Phone,
                Subject = TeacherDtoForPost.Subject
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            // Map the newly created Teacher entity back to TeacherDto for the response
            var createdTeacherDto = new TeacherDtoForPost
            {
                Name = teacher.Name,
                Email = teacher.Email,
                Phone = teacher.Phone,
                Subject = teacher.Subject
            };

            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, createdTeacherDto);
        }
        
        // GET: api/Teacher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDtoForGet>>> GetTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.FormClass) // Eagerly load FormClass
                .Select(t => new TeacherDtoForGet
                {
                    TeacherId = t.TeacherId,
                    Name = t.Name,
                    Email = t.Email,
                    Phone = t.Phone,
                    Subject = t.Subject,
                    FormClassId = t.FormClass != null ? t.FormClass.SchoolClassId : (int?)null, // Null check and correct property
                    FormClassName = t.FormClass != null ? t.FormClass.ClassName : null // Null check
                })
                .ToListAsync();

            return Ok(teachers);
        }

        // GET: api/Teacher/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDtoForGet>> GetTeacherById(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.FormClass) // Eagerly load FormClass
                .Where(t => t.TeacherId == id)
                .Select(t => new TeacherDtoForGet
                {
                    TeacherId = t.TeacherId,
                    Name = t.Name,
                    Email = t.Email,
                    Phone = t.Phone,
                    Subject = t.Subject,
                    FormClassId = t.FormClass != null ? t.FormClass.SchoolClassId : (int?)null, // Null check and correct property
                    FormClassName = t.FormClass != null ? t.FormClass.ClassName : null // Replaced null propagating operator
                })
                .FirstOrDefaultAsync();

            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, TeacherDtoForPost TeacherDtoForPost)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            // Update all fields of the teacher
            teacher.Name = TeacherDtoForPost.Name;
            teacher.Email = TeacherDtoForPost.Email;
            teacher.Phone = TeacherDtoForPost.Phone;
            teacher.Subject = TeacherDtoForPost.Subject;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Teachers.Any(t => t.TeacherId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTeacher(int id, [FromBody] JsonPatchDocument<Teacher> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(teacher, error =>
            {
                ModelState.AddModelError(error.AffectedObject.ToString(), error.ErrorMessage);
            });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TeacherDtoForGet>> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.FormClass)
                .FirstOrDefaultAsync(t => t.TeacherId == id);

            if (teacher == null)
            {
                return NotFound();
            }

            if (teacher.FormClass != null)
            {
                return BadRequest("Cannot delete this teacher because they are assigned as a FormTeacher.");
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}
