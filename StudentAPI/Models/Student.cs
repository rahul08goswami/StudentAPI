using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Gender { get; set; }
        public int Age { get; set; }
        [Column(TypeName = "varchar(500)")]
        
        public string Image_Path { get; set; }
    }
}
