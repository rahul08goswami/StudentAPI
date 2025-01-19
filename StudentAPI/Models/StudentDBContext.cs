using Microsoft.EntityFrameworkCore;

namespace StudentAPI.Models
{
    public class StudentDBContext:DbContext
    {
        public StudentDBContext()
        {
            
        }
        public StudentDBContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
    }
}
