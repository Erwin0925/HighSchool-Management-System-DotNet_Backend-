using Azure;
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
    public class SchoolClassController : ControllerBase
    {
        private readonly DataContext _context;

        public SchoolClassController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<SchoolClassDtoForPost>>CreateSchoolClass(SchoolClassDtoForPost SchoolClassDtoForPost)
        {
            if(SchoolClassDtoForPost.FormTeacherId != null)
            {
                var FormTeacher = await _context.Teachers.FindAsync(SchoolClassDtoForPost.FormTeacherId);
                if (FormTeacher == null)
                {
                    return NotFound($"Form Teacher with ID {SchoolClassDtoForPost.FormTeacherId} not found.");
                }
            }

            var SchoolClass = new SchoolClass
            {
                ClassName = SchoolClassDtoForPost.ClassName,
                FormTeacherId = SchoolClassDtoForPost.FormTeacherId
            };

            _context.SchoolClasses.Add(SchoolClass);
            await _context.SaveChangesAsync();

            var createdSchoolClassDto = new SchoolClassDtoForPost
            {
                ClassName = SchoolClass.ClassName,
                FormTeacherId = SchoolClass.FormTeacherId
            };
            return CreatedAtAction(nameof(GetSchoolClassesById), new { id = SchoolClass.SchoolClassId }, createdSchoolClassDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolClassDtoForGet>>> GetSchoolClasses()
        {
            var schoolClasses = await _context.SchoolClasses
                .Include(sc => sc.FormTeacher)
                .Include(sc => sc.Students)
                .Select(sc => new SchoolClassDtoForGet
                {
                    SchoolClassId = sc.SchoolClassId,
                    ClassName = sc.ClassName,
                    FormTeacherName = sc.FormTeacher != null ? sc.FormTeacher.Name : null, // Null check
                    StudentNames = sc.Students.Select(s => s.Name).ToList()
                })
                .ToListAsync();

            return Ok(schoolClasses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolClassDtoForGet>> GetSchoolClassesById(int id)
        {
            var schoolClass = await _context.SchoolClasses
                .Include (sc => sc.FormTeacher)
                .Include(sc => sc.Students)
                .FirstOrDefaultAsync(sc => sc.SchoolClassId == id);

            if (schoolClass == null)
            {
                return NotFound($"School class with ID {id} not found.");
            }

            var SchoolClassDto = new SchoolClassDtoForGet
            {
                SchoolClassId = schoolClass.SchoolClassId,
                ClassName = schoolClass.ClassName,
                FormTeacherName = schoolClass.FormTeacher?.Name,
                StudentNames = schoolClass.Students.Select(s => s.Name).ToList()
            };

            return Ok(SchoolClassDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolClass(int id, SchoolClassDtoForPost SchoolClassDto)
        {
            var schoolclass = await _context.SchoolClasses.FindAsync(id);
            if (schoolclass == null)
            {
                return NotFound($"SchoolClass with ID {id} not found.");
            }

            schoolclass.ClassName = SchoolClassDto.ClassName;
            schoolclass.FormTeacherId = SchoolClassDto.FormTeacherId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchClass(int id, [FromBody] JsonPatchDocument<SchoolClass> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document");
            }

            var schoolClass = await _context.SchoolClasses.FindAsync(id);
            if (schoolClass == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(schoolClass, error =>
            {
                ModelState.AddModelError(error.AffectedObject.ToString(), error.ErrorMessage);
            });

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SchoolClass>> DeleteSchoolClass(int id)
        {
            var schoolClass = await _context.SchoolClasses.FindAsync(id);
            if (schoolClass == null)
            {
                return NotFound();
            }

            _context.Remove(schoolClass);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }

    }
}
