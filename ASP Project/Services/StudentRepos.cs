using ASP_Project.Data;
using ASP_Project.Models;
using ASP_Project.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project.Services
{
	public class StudentRepos : IStudentRepository
	{
		private readonly SchoolContext _schoolContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public StudentRepos(SchoolContext appDbContext, UserManager<ApplicationUser> userManager,
		  RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
		{
			_schoolContext = appDbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public IEnumerable<Student> Students()
		{
			var students = _schoolContext.Students.AsNoTracking();
			return students;
		}

		public async Task<IdentityResult> CreateStudentAsync(string Username, string Password, string firstName, string middleName, string lastName)
		{
			var user = new ApplicationUser()
			{
				UserName = Username,
			};

			var result = await _userManager.CreateAsync(user, Password);

			if (result.Succeeded)
				result = await _userManager.AddToRoleAsync(user, "Student");

			if (result.Succeeded)
			{
				var student = new Student()
				{
					ApplicationUser = user,
					Username = Username,
					FirstName = firstName,
					MiddleName = middleName,
					LastName = lastName,
					CreditsTaken = 0
				};

				_schoolContext.Students.Add(student);
				if (_schoolContext.SaveChanges() == 0)
				{
					var awaiter = _userManager.DeleteAsync(user).GetAwaiter();
				}
			}

			if (!result.Succeeded)
			{
				var awaiter = _userManager.DeleteAsync(user).GetAwaiter();
			}
			await _signInManager.SignInAsync(user, false);
			return result;
		}

		public Task<IdentityResult> DeleteStudentAsync(int StudentId)
		{
			throw new NotImplementedException();
		}

		public Student GetStudentByUser(ApplicationUser appUser)
		=> _schoolContext.Students.FirstOrDefault(a => a.ApplicationUser == appUser);
	}
}