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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_Project.Controllers
{
	public class TeacherController : Controller
	{
		private readonly SchoolContext _schoolcontext;
		private readonly ITeacherRepository _teacherRepository;
		private readonly IAdminRepository _adminRepository;
		private readonly UserManager<ApplicationUser> _userManager;

		public TeacherController(SchoolContext context,
			ITeacherRepository teacherRepository,
			UserManager<ApplicationUser> userManager,
			IAdminRepository adminRepository
			)
		{
			_schoolcontext = context;
			_teacherRepository = teacherRepository;
			_userManager = userManager;
			_adminRepository = adminRepository;
		}

		// GET: /<controller>/
		[Authorize(Roles = "Admin")]
		public IActionResult Teachers()
		{
			IEnumerable<Teacher> teachers = _teacherRepository.Teachers();
			return View(teachers);
		}

		[Authorize(Roles = "Teacher")]
		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles = "Teacher")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var teacher = await _schoolcontext.Teacher.SingleOrDefaultAsync(m => m.TeacherID == id);
			if (teacher == null)
			{
				return NotFound();
			}
			return View(teacher);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Teacher")]
		public async Task<IActionResult> Edit(int id, [Bind("TeacherID,FirstName,MiddleName,LastName,Username")] Teacher teacher)
		{
			if (id != teacher.TeacherID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_schoolcontext.Update(teacher);
					await _userManager.UpdateAsync(teacher.ApplicationUser);
					await _schoolcontext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TeacherExists(teacher.TeacherID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Teachers));
			}
			return View(teacher);
		}

		[Authorize(Roles = "Teacher")]
		public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return NotFound();
			}

			var teacher = await _schoolcontext.Teacher
				.AsNoTracking()
				.SingleOrDefaultAsync(m => m.TeacherID == id);
			if (teacher == null)
			{
				return NotFound();
			}

			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return View(teacher);
		}

		// POST: Students/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Teacher")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var teacher = await _schoolcontext.Teacher.SingleOrDefaultAsync(m => m.TeacherID == id);
			await _userManager.DeleteAsync(teacher.ApplicationUser);
			_schoolcontext.Teacher.Remove(teacher);
			await _schoolcontext.SaveChangesAsync();
			return RedirectToAction(nameof(Teachers));
		}

		private bool TeacherExists(int teacherID)
		{
			return _schoolcontext.Teacher.Any(e => e.TeacherID == teacherID);
		}

		[Authorize(Roles = "Teacher")]
		public IActionResult Manage()
		{
			IEnumerable<string> teachers = _teacherRepository.TeacherNames();
			return View(new CourseViewModel()
			{
				Teachers = teachers.ToList()
			});
		}

		[Authorize(Roles = "Teacher")]
		[HttpPost]
		public async Task<IActionResult> Manage(CourseViewModel courseViewModel)
		{
			IEnumerable<string> teachers = _teacherRepository.TeacherNames();
			if (ModelState.IsValid)
			{
				var teacher = _schoolcontext.Teacher.Single(t => t.FirstName == courseViewModel.TeacherName);
				Course course = new Course()
				{
					CodeID = courseViewModel.CodeID,
					Name = courseViewModel.Name,
					NumOfCredits = courseViewModel.NumOfCredits,
					TeacherID = teacher.TeacherID
				};
				await _schoolcontext.Course.AddAsync(course);
				if (await _schoolcontext.SaveChangesAsync() == 0)
				{
					return View(new CourseViewModel()
					{
						Teachers = teachers.ToList()
					});
				}
				return RedirectToAction("Index", "Teacher");
			}
			return View(new CourseViewModel()
			{
				Teachers = teachers.ToList()
			});
		}

		[Authorize(Roles = "Teacher")]
		public IActionResult Courses(CourseViewModel courseViewModel)
		{
			IEnumerable<Course> courses = _adminRepository.Courses();
			return View(courses);
		}

		[Authorize(Roles = "Teacher")]
		public async Task<IActionResult> EditCourse(string Code)
		{
			if (Code == null)
			{
				return NotFound();
			}
			var course = await _schoolcontext.Course.SingleOrDefaultAsync(m => m.CodeID == Code);
			if (course == null)
			{
				return NotFound();
			}
			return View(course);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Teacher")]
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
					_schoolcontext.Update(course);
					await _schoolcontext.SaveChangesAsync();
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
			return _schoolcontext.Course.Any(e => e.CodeID == Code);
		}

		[Authorize(Roles = "Teacher")]
		public async Task<IActionResult> Grade()
		{
			ClaimsPrincipal currentUser = User;
			var user = await _userManager.GetUserAsync(currentUser);
			var teacher = _teacherRepository.GetTeacherByUser(user);
			await _schoolcontext.Entry(teacher).Collection(t => t.Courses).LoadAsync();

			return View(new TeacherGradingModel()
			{
				Courses = teacher.Courses
			});
		}

		public async Task<string> GetStudents(string id)
		{
			List<Student> students = new List<Student>();
			var enrollments = _schoolcontext.Enrollments;
			string result = "";
			foreach (var item in enrollments)
			{
				await _schoolcontext.Entry(item).Reference(e => e.Course).LoadAsync();
				await _schoolcontext.Entry(item).Reference(e => e.Student).LoadAsync();

				if (item.Course.CodeID == id)
				{
					result += "<option value='" + item.Student.StudentID + "'>" + item.Student.FirstName + "</option>";
				}
			}
			return result;
		}

		public async Task<IActionResult> SetGrades(TeacherGradingModel teacherGradingModel)
		{
			var enrollments = _schoolcontext.Enrollments;
			foreach (var item in enrollments)
			{
				await _schoolcontext.Entry(item).Reference(e => e.Student).LoadAsync();
				if (item.Student.StudentID == teacherGradingModel.StudentID)
				{
					item.Grade = teacherGradingModel.Grade;
					_schoolcontext.Enrollments.Update(item);
					break;
				}
			}
			await _schoolcontext.SaveChangesAsync();
			return RedirectToAction("Index", "Teacher");
		}
	}
}