using CoursesOnline.Domain.Entities;
using CoursesOnline.Domain.Enums;
using CoursesOnline.Domain.Interfaces;
using CoursesOnline.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CoursesOnline.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Course?> GetByIdAsync(Guid id)
        {
            return await _context.Courses
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> SearchAsync(
            string? query,
            CourseStatus? status,
            int page,
            int pageSize)
        {
            IQueryable<Course> courses = _context.Courses;

            if (!string.IsNullOrWhiteSpace(query))
                courses = courses.Where(c => c.Title.Contains(query));

            if (status.HasValue)
                courses = courses.Where(c => c.Status == status);

            return await courses
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
    }
}