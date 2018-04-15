using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP_Project.Data;
using Microsoft.AspNetCore.Mvc;


namespace ASP_Project.Controllers
{
    public class CourseController : Controller
    {
        private readonly SchoolContext _context;

        public CourseController(SchoolContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        private bool CourseExists(string id)
        {
            return _context.Course.Any(e => e.CodeID.Equals(id) );
        }
    }
    
}
