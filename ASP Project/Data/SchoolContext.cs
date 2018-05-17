using ASP_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASP_Project.Data
{
    public class SchoolContext : IdentityDbContext<ApplicationUser>
    {
		public SchoolContext()
		{
		}

		public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);

            builder.Entity<Enrollment>(b =>
            {
                b.HasKey(sc => new { sc.StudentID, sc.CourseID});

                b.HasOne(sc => sc.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(sc => sc.StudentID)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(sc => sc.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(sc => sc.CourseID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            

            builder.Entity<Course>()
                .HasIndex(c => c.Name)
                .IsUnique();

            
        }

    }
}