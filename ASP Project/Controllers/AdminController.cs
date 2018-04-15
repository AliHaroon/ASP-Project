using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP_Project.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_Project.Controllers
{

    public class AdminController : Controller
    {
        private readonly SchoolContext _context;

        public AdminController(SchoolContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.AdminID == id);
        }
    }
}
