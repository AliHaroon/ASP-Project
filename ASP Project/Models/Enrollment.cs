using System.ComponentModel.DataAnnotations;

namespace ASP_Project.Models
{
    public class Enrollment
    {
        [Key] public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public float Grade { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}