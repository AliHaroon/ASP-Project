using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP_Project.Models
{
    public class Course
    {
        [Key] public string CodeID { get; set; }
        public int NumOfCredits { get; set; }
        public string Name { get; set; }
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}