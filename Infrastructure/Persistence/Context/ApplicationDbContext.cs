using CoursesOnline.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoursesOnline.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>()
                .HasMany(c => c.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId);

            builder.Entity<Lesson>()
                .HasIndex(l => new { l.CourseId, l.Order })
                .IsUnique();

            builder.Entity<Course>()
                .HasQueryFilter(c => !c.IsDeleted);

            builder.Entity<Lesson>()
                .HasQueryFilter(l => !l.IsDeleted);
        }
    }
}