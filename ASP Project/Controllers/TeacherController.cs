using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP_Project.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_Project.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SchoolContext _context;

        public TeacherController(SchoolContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.TeacherID == id);
        }
    }
}
