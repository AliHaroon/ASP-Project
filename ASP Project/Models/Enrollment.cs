using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_Project.Models
{
    public class Enrollment
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key] public int EnrollmentID { get; set; }
        public string CourseID { get; set; }
        public int StudentID { get; set; }
        public float Grade { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}