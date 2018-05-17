using ASP_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project.ViewModels
{
    public class CourseViewModel
    {
		[Required]
		public string CodeID { get; set; }
		[Required]
		public int NumOfCredits { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string TeacherName { get; set; }
		public List<string> Teachers { get; set; } 
	}
}
