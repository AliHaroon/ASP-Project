using ASP_Project.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP_Project.Services.Interfaces
{
	public interface ITeacherRepository
	{
		IEnumerable<Teacher> Teachers();

		Task<IdentityResult> CreateTeacherAsync(string Username, string Password, string FirstName, string MiddleName, string LastName);

		Task<IdentityResult> DeleteTeacherAsync(int TeacherId);

		Teacher GetTeacherByUser(ApplicationUser appUser);

		IEnumerable<string> TeacherNames();
	}
}