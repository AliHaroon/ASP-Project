using ASP_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project.AccountViewModels
{
    public class LoginViewModel
    {
        public Student student { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
