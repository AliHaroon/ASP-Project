using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASP_Project.Models.RegisterViewModel;

namespace ASP_Project.Models
{

    
    public class ApplicationUser: IdentityUser
    {
        public Types Type { get; set; }

     
    }
}
