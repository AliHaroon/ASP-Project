using ASP_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project.ViewModels
{
    public class TeacherGradingModel
    {
		public IEnumerable<Course> Courses { get; set; }
		public string CourseID { get; set; }
		public IEnumerable<Student> Students { get; set; }
		public string StudentID { get; set; }
		public float Grade { get; set; }

    }
}
