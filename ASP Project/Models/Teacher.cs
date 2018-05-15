using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP_Project.Models
{
	public class Teacher
	{
		[Key] public int TeacherID { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
		public virtual List<Course> Courses { get; set; } = new List<Course>();
		public string ApplicationUserId { get; set; }
	}
}