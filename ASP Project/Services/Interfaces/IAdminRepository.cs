using ASP_Project.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP_Project.Services.Interfaces
{
	public interface IAdminRepository
	{
		IEnumerable<Admin> Admins();

		Task<IdentityResult> CreateAdminAsync(string Username, string Password, string FirstName, string MiddleName, string LastName);

		Task<IdentityResult> DeleteAdminAsync(int adminId);

		Admin GetAdminByUser(ApplicationUser appUser);

		IEnumerable<Course> Courses();
	}
}