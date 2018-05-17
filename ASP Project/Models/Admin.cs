using System.ComponentModel.DataAnnotations;

namespace ASP_Project.Models
{
	public class Admin
	{
		[Key] public int AdminID { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
		public string ApplicationUserId { get; set; }
	}
}