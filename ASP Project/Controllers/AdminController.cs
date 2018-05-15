using ASP_Project.Data;
using ASP_Project.Models;
using ASP_Project.Services.Interfaces;
using ASP_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP_Project.Controllers
{
	public class AdminController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SchoolContext _schoolContext;
		private readonly IAdminRepository _adminRepos;
		private readonly ITeacherRepository _teacherRepository;

		public AdminController(UserManager<ApplicationUser> userManager,
			SchoolContext schoolContext,
			IAdminRepository adminRepos,
			ITeacherRepository teacherRepository
			)
		{
			_userManager = userManager;
			_schoolContext = schoolContext;
			_adminRepos = adminRepos;
			_teacherRepository = teacherRepository;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Index()
		{
			ClaimsPrincipal currentUser = User;
			var user = await _userManager.GetUserAsync(currentUser);
			var admin = _adminRepos.GetAdminByUser(user);

			return View(new AdminViewModel()
			{
				FirstName = admin.FirstName,
				LastName = admin.LastName,
				MiddleName = admin.MiddleName
			});
		}

		[HttpGet]
		public IActionResult Manage()
		{
			IEnumerable<string> teachers = _teacherRepository.TeacherNames();
			return View(new CourseViewModel()
			{
				Teachers = teachers.ToList()
			});
		}

		[HttpPost]
		public async Task<IActionResult> Manage(CourseViewModel courseViewModel)
		{
			IEnumerable<string> teachers = _teacherRepository.TeacherNames();
			if (ModelState.IsValid)
			{
				var teacher = _schoolContext.Teacher.Single(t => t.FirstName == courseViewModel.TeacherName);
				Course course = new Course()
				{
					CodeID = courseViewModel.CodeID,
					Name = courseViewModel.Name,
					NumOfCredits = courseViewModel.NumOfCredits,
					TeacherID = teacher.TeacherID
				};
				await _schoolContext.Course.AddAsync(course);
				if (await _schoolContext.SaveChangesAsync() == 0)
				{
					return View(new CourseViewModel()
					{
						Teachers = teachers.ToList()
					});
				}
				return RedirectToAction("Index", "Admin");
			}
			return View(new CourseViewModel()
			{
				Teachers = teachers.ToList()
			});
		}

		public IActionResult Courses(CourseViewModel courseViewModel)
		{
			IEnumerable<Course> courses = _adminRepos.Courses();
			return View(courses);
		}

		public async Task<IActionResult> EditCourse(string Code)
		{
			if (Code == null)
			{
				return NotFound();
			}
			var course = await _schoolContext.Course.SingleOrDefaultAsync(m => m.CodeID == Code);
			if (course == null)
			{
				return NotFound();
			}
			return View(course);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditCourse(string Code, [Bind("CodeID", "NumOfCredits", "Name", "TeacherID")] Course course)
		{
			if (Code != course.CodeID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_schoolContext.Update(course);
					await _schoolContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CourseExists(course.CodeID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(course);
		}
		private bool CourseExists(string Code)
		{
			return _schoolContext.Course.Any(e => e.CodeID == Code);
		}
	}
}