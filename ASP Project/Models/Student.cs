using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP_Project.Models
{
	public class Student
	{
		[Key] public int StudentID { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public int CreditsTaken { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
		public string ApplicationUserId { get; set; }
		public ICollection<Enrollment> Enrollments { get; set; }
	}
}