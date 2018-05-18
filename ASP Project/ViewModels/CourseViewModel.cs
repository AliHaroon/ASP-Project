using ASP_Project.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
		public int TeacherId { get; set; }

		public IEnumerable<Teacher> Teachers { get; set; }
	}
}