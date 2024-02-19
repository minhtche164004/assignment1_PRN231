using Exercise2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exe2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly uniContext _context;

        public StudentsController(uniContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("student null roi");
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            if (updatedStudent == null)
            {
                return BadRequest();
            }

            var existingStudent = await _context.Students.FindAsync(id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            // Update properties of existingStudent with values from updatedStudent
            existingStudent.StudentName = updatedStudent.StudentName;
            existingStudent.Photo = updatedStudent.Photo;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(existingStudent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentList()
        {
            var studentList = await _context.Students.Select(s => new { s.StudentId, s.StudentName, s.Photo }).ToListAsync();

            if (studentList == null)
            {
                return NotFound();
            }

            return Ok(studentList);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchStudents(string keyword)
        {
            try
            {
                var students = await _context.Students
                    .Where(s => s.StudentName.Contains(keyword) || s.StudentId.ToString() == keyword)
                    .ToListAsync();

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



    }
}