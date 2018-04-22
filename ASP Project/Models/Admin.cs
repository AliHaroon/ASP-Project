using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project.Models
{
    public class Admin
    {
       [Key] public int AdminID { get; set; }
        public string Username { get; set; }
        public virtual ApplicationUser AppUser { get; set; }
    }
}
