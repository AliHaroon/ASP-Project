using Microsoft.AspNetCore.Identity;

namespace ASP_Project.Models
{


    public class ApplicationUser : IdentityUser
    {
		public virtual Admin Admin { get; set; }
		public virtual Student Student { get; set; }
		public virtual Teacher Teacher { get; set; }

	}
}