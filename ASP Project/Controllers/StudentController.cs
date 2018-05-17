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
	public class StudentController : Controller
	{
		private readonly IStudentRepository _studentRepository;
		private readonly SchoolContext _schoolContext;
		private readonly IAdminRepository _adminRepository;
		private readonly UserManager<ApplicationUser> _userManager;

		public StudentController(IStudentRepository studentRepository,
			SchoolContext schoolContext,
			UserManager<ApplicationUser> userManager,
			IAdminRepository adminRepository
			)
		{
			_studentRepository = studentRepository;
			_schoolContext = schoolContext;
			_userManager = userManager;
			_adminRepository = adminRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Students(StudentViewModel studentViewModel)
		{
			IEnumerable<Student> students = _studentRepository.Students();
			return View(students);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var student = await _schoolContext.Students.SingleOrDefaultAsync(m => m.StudentID == id);
			if (student == null)
			{
				return NotFound();
			}
			return View(student);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("StudentID,FirstName,MiddleName,LastName,Username")] Student student)
		{
			if (id != student.StudentID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_schoolContext.Update(student);
					await _userManager.UpdateAsync(student.ApplicationUser);
					await _schoolContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!StudentExists(student.StudentID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Students));
			}
			return View(student);
		}

		public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return NotFound();
			}

			var student = await _schoolContext.Students
				.AsNoTracking()
				.SingleOrDefaultAsync(m => m.StudentID == id);
			if (student == null)
			{
				return NotFound();
			}

			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return View(student);
		}

		// POST: Students/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var student = await _schoolContext.Students.SingleOrDefaultAsync(m => m.StudentID == id);
			await _userManager.DeleteAsync(student.ApplicationUser);
			_schoolContext.Students.Remove(student);
			await _schoolContext.SaveChangesAsync();
			return RedirectToAction(nameof(Students));
		}

		private bool StudentExists(int id)
		{
			return _schoolContext.Students.Any(e => e.StudentID == id);
		}

		[HttpGet]
		public IActionResult RegisterCourse()
		{
			var courses = _adminRepository.Courses();
			return View(new StudentRegisterViewModel()
			{
				Courses = courses
			});
		}

		[HttpPost]
		public async Task<IActionResult> RegisterCourse(StudentRegisterViewModel srvm)
		{
			var courses = _adminRepository.Courses();
			if (ModelState.IsValid)
			{
				ClaimsPrincipal currentUser = User;
				var user = await _userManager.GetUserAsync(currentUser);
				var student = _studentRepository.GetStudentByUser(user);
				var course = _schoolContext.Course.SingleOrDefault(c => c.CodeID == srvm.CourseID);
				var check = _schoolContext.Enrollments.Where(e => e.StudentID == student.StudentID).Any(e => e.CourseID == srvm.CourseID);
				if (check)
				{
					ModelState.AddModelError("", "You can't register the same Course more than once");
					return View(new StudentRegisterViewModel()
					{
						Courses = courses
					});
				}
				var enrollment = new Enrollment()
				{
					CourseID = course.CodeID,
					StudentID = student.StudentID
				};
				int NewCredits = student.CreditsTaken + course.NumOfCredits;
				if (NewCredits > 30)
				{
					ModelState.AddModelError("", "You can't register to more than 30 credits worth of courses");
					return View(new StudentRegisterViewModel()
					{
						Courses = courses
					});
				}
				else
				{
					student.CreditsTaken = NewCredits;
				}
				_schoolContext.Students.Update(student);
				_schoolContext.Enrollments.Add(enrollment);
				if (_schoolContext.SaveChanges() == 0)
				{
					ModelState.AddModelError("", "Something went wrong while updating database, please try again later");
					return View(new StudentRegisterViewModel()
					{
						Courses = courses
					});
				}
			}
			else
			{
				ModelState.AddModelError("", "Something went wrong while updating database, please try again later");
				return View(new StudentRegisterViewModel()
				{
					Courses = courses
				});
			}

			return View(new StudentRegisterViewModel()
			{
				Courses = courses
			});
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Drop()
		{
			ClaimsPrincipal currentUser = User;
			var user = await _userManager.GetUserAsync(currentUser);
			var student = _studentRepository.GetStudentByUser(user);
			var enroll = _schoolContext.Enrollments.Where(e => e.StudentID == student.StudentID).Select(e => e.CourseID);
			return View(new StudentCourseViewModel()
			{
				Enrollment = enroll
			});
		}

		[HttpPost]
		[Authorize(Roles = "Student")]
		public IActionResult Drop(StudentCourseViewModel scvm)
		{
			var enrollment = _schoolContext.Enrollments.SingleOrDefault(e => e.CourseID == scvm.CourseID);
			_schoolContext.Enrollments.Remove(enrollment);
			_schoolContext.SaveChanges();
			return RedirectToAction("Index", "Student");
		}

		[HttpGet]
		[Authorize(Roles = "Student")]
		public async Task<IActionResult> ListCourses()
		{
			ClaimsPrincipal currentUser = User;
			var user = await _userManager.GetUserAsync(currentUser);
			var student = _studentRepository.GetStudentByUser(user);
			await _schoolContext.Entry(student).Collection(x => x.Enrollments).LoadAsync();
			List<Course> courses = new List<Course>();
			foreach (Enrollment enrollment in student.Enrollments)
			{
				await _schoolContext.Entry(enrollment).Reference(e => e.Course).LoadAsync();
				courses.Add(enrollment.Course);
			}

			return View(courses);
		}

		[Authorize(Roles = "Student")]
		public async Task<IActionResult> ListGrades()
		{
			ClaimsPrincipal currentUser = User;
			var user = await _userManager.GetUserAsync(currentUser);
			var student = _studentRepository.GetStudentByUser(user);
			await _schoolContext.Entry(student).Collection(x => x.Enrollments).LoadAsync();
			foreach (Enrollment enrollment in student.Enrollments)
			{
				await _schoolContext.Entry(enrollment).Reference(e => e.Course).LoadAsync();
			}
			return View(student);
		}

		

	}
}