using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASP_Project.Data;
using ASP_Project.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASP_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var host = BuildWebHost(args);

			using (var scope = host.Services.CreateScope())
			{
				var serviceProvider = scope.ServiceProvider;
				try
				{
					
					var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
					UserRoleSeed.Seed(roleManager);
					var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
					UserRoleSeed.SeedUser(userManager);
					var school = serviceProvider.GetRequiredService<SchoolContext>();
					UserRoleSeed.SeedToDb(school,userManager);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("OtherText");
				}
			}

			host.Run();
        }

		public static IWebHost BuildWebHost(string[] args) =>
		   WebHost.CreateDefaultBuilder(args)
			   .UseStartup<Startup>()
			   .Build();
	}
}
