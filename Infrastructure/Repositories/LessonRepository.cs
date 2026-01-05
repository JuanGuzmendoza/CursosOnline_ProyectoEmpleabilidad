using CoursesOnline.Domain.Entities;
using CoursesOnline.Domain.Interfaces;
using CoursesOnline.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CoursesOnline.Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.Lessons
                .Where(l => l.CourseId == courseId)
                .OrderBy(l => l.Order)
                .ToListAsync();
        }

        public async Task<bool> ExistsWithOrderAsync(Guid courseId, int order)
        {
            return await _context.Lessons
                .AnyAsync(l => l.CourseId == courseId && l.Order == order);
        }

        public async Task AddAsync(Lesson lesson)
        {
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task<Lesson?> GetByIdAsync(Guid lessonId)
        {
            return await _context.Lessons
                .FirstOrDefaultAsync(l => l.Id == lessonId);
        }
    }
}