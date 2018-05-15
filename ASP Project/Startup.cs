using ASP_Project.Data;
using ASP_Project.Models;
using ASP_Project.Services;
using ASP_Project.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASP_Project
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<SchoolContext>(options =>
			   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddIdentity<ApplicationUser, IdentityRole>()
		.AddEntityFrameworkStores<SchoolContext>()
		.AddDefaultTokenProviders();

			services.AddTransient<IAdminRepository, AdminRepos>();
			services.AddTransient<ITeacherRepository, TeacherRepo>();
			services.AddTransient<IStudentRepository, StudentRepos>();
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}