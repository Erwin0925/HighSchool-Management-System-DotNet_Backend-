using System.Numerics;
using System.Reflection;
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
    public class StudentController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<StudentDtoForGet>> CreateStudent(StudentDtoForPost studentDto)
        {
            var schoolClass = await _context.SchoolClasses.FindAsync(studentDto.SchoolClassId);
            if (schoolClass == null)
            {
                return NotFound($"School class with ID {studentDto.SchoolClassId} not found.");
            }

            var student = new Student
            {
                Name = studentDto.Name,
                Dob = studentDto.Dob,
                Gender = studentDto.Gender,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                SchoolClassId = studentDto.SchoolClassId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var createdStudentDto = new StudentDtoForPost
            {
                Name = student.Name,
                Dob = (DateTime)student.Dob,
                Gender = student.Gender,
                Email = student.Email,
                Phone = student.Phone,
                SchoolClassId = (int)student.SchoolClassId,
            };

            return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, createdStudentDto);
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students
                        .Include(s => s.SchoolClass)
                        .ThenInclude(sc => sc.FormTeacher)
                        .Select(s => new StudentDtoForGet
                        {
                            StudentId = s.StudentId,
                            Name = s.Name,
                            Dob = (DateTime)s.Dob,
                            Gender = s.Gender,
                            Email = s.Email,
                            Phone = s.Phone,
                            SchoolClassId = s.SchoolClass.SchoolClassId,
                            SchoolClassName = s.SchoolClass.ClassName,
                            FormTeacherName = s.SchoolClass.FormTeacher != null ? s.SchoolClass.FormTeacher.Name : null // Null check
                        })
                        .ToListAsync();

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDtoForGet>> GetStudentById(int id)
        {
            var student = await _context.Students
                .Include(s => s.SchoolClass)
                .ThenInclude(sc => sc.FormTeacher)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            var studentDto = new StudentDtoForGet
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Dob = (DateTime)student.Dob,
                Gender = student.Gender,
                Email = student.Email,
                Phone = student.Phone,
                SchoolClassId = student.SchoolClass.SchoolClassId,
                SchoolClassName = student.SchoolClass.ClassName,
                FormTeacherName = student.SchoolClass.FormTeacher?.Name
            };

            return Ok(studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentDtoForPost studentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            student.Name = studentDto.Name;
            student.Dob = studentDto.Dob;
            student.Gender = studentDto.Gender;
            student.Email = studentDto.Email;
            student.Phone = studentDto.Phone;
            student.SchoolClassId = studentDto.SchoolClassId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(s => s.StudentId == id))
                {
                    return NotFound($"Student with ID {id} not found.");
                }
                throw;
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStudent(int id, [FromBody] JsonPatchDocument<Student> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document");
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(student, error =>
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
        public async Task<ActionResult<StudentDtoForGet>> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}
