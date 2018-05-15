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
	public class TeacherRepo : ITeacherRepository
	{
		private readonly SchoolContext _schoolContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public TeacherRepo(SchoolContext appDbContext, UserManager<ApplicationUser> userManager,
		  RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
		{
			_schoolContext = appDbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public IEnumerable<Teacher> Teachers()
		{
			var teachers = _schoolContext.Teacher.AsNoTracking();
			return teachers;
		}

		public IEnumerable<string> TeacherNames() => _schoolContext.Teacher.Select(t => t.FirstName);

		public async Task<IdentityResult> CreateTeacherAsync(string Name, string Password, string firstName, string middleName, string lastName)
		{
			var user = new ApplicationUser()
			{
				UserName = Name
			};

			var result = await _userManager.CreateAsync(user, Password);

			if (result.Succeeded)
				result = await _userManager.AddToRoleAsync(user, "Teacher");

			if (result.Succeeded)
			{
				var teacher = new Teacher()
				{
					ApplicationUser = user,
					Username = Name,
					FirstName = firstName,
					MiddleName = middleName,
					LastName = lastName
				};

				_schoolContext.Teacher.Add(teacher);
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

		public Task<IdentityResult> DeleteTeacherAsync(int TeacherId)
		{
			throw new NotImplementedException();
		}

		public Teacher GetTeacherByUser(ApplicationUser appUser)
		=> _schoolContext.Teacher.FirstOrDefault(a => a.ApplicationUser == appUser);
	}
}