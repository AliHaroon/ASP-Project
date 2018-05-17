using ASP_Project.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project.Services.Interfaces
{
    public interface IStudentRepository
    {
		IEnumerable<Student> Students();

		Task<IdentityResult> CreateStudentAsync(string Username, string Password, string FirstName, string MiddleName, string LastName);

		Task<IdentityResult> DeleteStudentAsync(int StudentId);

		Student GetStudentByUser(ApplicationUser appUser);
	}
}
