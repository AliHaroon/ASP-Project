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
	public class AdminRepos : IAdminRepository
	{
		private readonly SchoolContext _schoolContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AdminRepos(SchoolContext appDbContext, UserManager<ApplicationUser> userManager,
		  RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
		{
			_schoolContext = appDbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public IEnumerable<Admin> Admins()
		{
			var admins = _schoolContext.Admin;
			foreach (var admin in admins)
			{
				_schoolContext.Entry(admin).Reference(a => a.ApplicationUser).Load();
			}
			return admins;
		}

		public IEnumerable<Course> Courses()
		{
			var courses = _schoolContext.Course.AsNoTracking();
			return courses;
		}

		public async Task<IdentityResult> CreateAdminAsync(string Name, string Password, string firstName, string middleName, string lastName)
		{
			var user = new ApplicationUser()
			{
				UserName = Name,
			};

			var result = await _userManager.CreateAsync(user, Password);

			if (result.Succeeded)
				result = await _userManager.AddToRoleAsync(user, "Admin");

			if (result.Succeeded)
			{
				var admin = new Admin()
				{
					ApplicationUser = user,
					Username = Name,
					FirstName = firstName,
					MiddleName = middleName,
					LastName = lastName
				};

				_schoolContext.Admin.Add(admin);
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

		public Task<IdentityResult> DeleteAdminAsync(int adminId)
		{
			throw new NotImplementedException("Something went wrong");
		}

		public Admin GetAdminByUser(ApplicationUser appUser)
			=> _schoolContext.Admin.FirstOrDefault(a => a.ApplicationUser == appUser);
	}
}