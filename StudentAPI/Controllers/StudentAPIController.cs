using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentAPIController : ControllerBase
    {
        private readonly StudentDBContext context;

        public StudentAPIController(StudentDBContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            var data = await context.Students.ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]      
        public async Task<ActionResult<Student>> GetStudentByID(int id)
        {
            //var student =  context.Students.Where(st => st.ID == id).FirstOrDefault();
            var student = await context.Students.FindAsync(id);
            //var student =  context.Students.Where(st => st.ID == id).First();
            if (student is null)
            {
                return NotFound();
            }
            return student;
        }
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();
            return Ok(student);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> EditStudent(int id, Student student)
        {
            if (id != student.ID)
            {
                return BadRequest();
            }
            context.Entry(student).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(student);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var std = await context.Students.FindAsync(id);
            if (std is null)
            {
                return NotFound();
            }
            //context.Students.Remove(std);
            context.Entry(std).State = EntityState.Deleted; //This works
            await context.SaveChangesAsync();
            return Ok(std);

        }


    }
}
