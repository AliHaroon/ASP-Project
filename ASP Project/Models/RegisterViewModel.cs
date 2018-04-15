
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASP_Project.Models;

namespace ASP_Project.Models
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

    [Required]
    public virtual int TypeId
    {
        get
        {
            return (int)this.Type;
        }
        set
        {
            Type = (Types)value;
        }
    }
    [EnumDataType(typeof(Types))]
    public Types Type { get; set; }

   public enum Types
        {
            Admin = 1,
            Teacher = 2,
            Student = 3
        }

        [EnumDataType(typeof(Type))]
    public Type PhoneType { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
