using ASP_Project.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ASP_Project.Data
{
	public static class UserRoleSeed
	{
		public static void Seed(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.RoleExistsAsync("Admin").Result)
			{
				IdentityRole role = new IdentityRole
				{
					Name = "Admin"
				};
				IdentityResult roleResult = roleManager.
				CreateAsync(role).Result;
				if (roleResult.Succeeded)
					System.Diagnostics.Debug.WriteLine("SomeText");
				else
					System.Diagnostics.Debug.WriteLine("Other text");
			}
			if (!roleManager.RoleExistsAsync("Teacher").Result)
			{
				IdentityRole role = new IdentityRole
				{
					Name = "Teacher"
				};
				IdentityResult roleResult = roleManager.
				CreateAsync(role).Result;
			}
			if (!roleManager.RoleExistsAsync("Student").Result)
			{
				IdentityRole role = new IdentityRole();
				role.Name = "Student";
				IdentityResult roleResult = roleManager.
				CreateAsync(role).Result;
			}
		}

		public static void SeedUser(UserManager<ApplicationUser> userManager)
		{
			if (userManager.FindByNameAsync("DragonKnight97").Result == null)
			{
				ApplicationUser user = new ApplicationUser();
				user.UserName = "DragonKnight97";

				IdentityResult result = userManager.CreateAsync
				(user, "Kazool@1234").Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(user, "Admin").Wait();
				}
			}
		}

		public static void SeedToDb(this SchoolContext schoolContext, UserManager<ApplicationUser> userManager)
		{
			if (schoolContext.Admin.SingleOrDefault(a => a.Username == "DragonKnight97") == null)
			{
				var user = userManager.FindByNameAsync("DragonKnight97").Result;

				var admin = new Admin()
				{
					ApplicationUser = user,
					Username = "DragonKnight97",
					FirstName = "Ali",
					MiddleName = "Fadi",
					LastName = "Haroon"
				};

				schoolContext.Add(admin);
				schoolContext.SaveChanges();
			}
		}
	}
}