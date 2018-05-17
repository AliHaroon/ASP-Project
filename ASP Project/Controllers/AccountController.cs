using ASP_Project.AccountViewModels;
using ASP_Project.Models;
using ASP_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ASP_Project.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IAdminRepository _adminRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly ITeacherRepository _teacherRepository;
		private readonly ILogger _logger;

		public AccountController(
					UserManager<ApplicationUser> userManager,
					SignInManager<ApplicationUser> signInManager,
					IAdminRepository adminRepository,
					ITeacherRepository teacherRepository,
					IStudentRepository studentRepository,
					ILogger<AccountController> logger)
		{
			_adminRepository = adminRepository;
			_userManager = userManager;
			_signInManager = signInManager;
			_teacherRepository = teacherRepository;
			_studentRepository = studentRepository;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}
		
		public IActionResult Login(string returnUrL)
		{
			return View(new LoginViewModel()
			{
				ReturnUrl = returnUrL
			});
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(loginViewModel);
			}

			var user = await _userManager.FindByNameAsync(loginViewModel.Username);
			if (user != null)
			{
				var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
				if (result.Succeeded)
				{
					if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
					{
						if (await _userManager.IsInRoleAsync(user, "Admin"))
							return RedirectToAction("Index", "Admin");
						if (await _userManager.IsInRoleAsync(user, "Teacher"))
							return RedirectToAction("Index", "Teacher");
						if (await _userManager.IsInRoleAsync(user, "Student"))
							return RedirectToAction("Index", "Student");
					}
					return View();
				}
			}

			ModelState.AddModelError("", "Username or Password are wrong");
			return View(loginViewModel);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid)
			{
				if (registerViewModel.Role.Equals("Admin"))
					await _adminRepository.CreateAdminAsync(registerViewModel.UserName,
						registerViewModel.Password,
						registerViewModel.FirstName,
						registerViewModel.MiddleName,
						registerViewModel.LastName
						);
				else if (registerViewModel.Role.Equals("Teacher"))
					await _teacherRepository.CreateTeacherAsync(registerViewModel.UserName,
						registerViewModel.Password,
						registerViewModel.FirstName,
						registerViewModel.MiddleName,
						registerViewModel.LastName
						);
				else
					await _studentRepository.CreateStudentAsync(registerViewModel.UserName,
						registerViewModel.Password,
						registerViewModel.FirstName,
						registerViewModel.MiddleName,
						registerViewModel.LastName
						);
				return RedirectToAction("Index", "Home");
			}

			return View(registerViewModel);
		}

		public async Task<IActionResult> Logout(string returnUrL)
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}