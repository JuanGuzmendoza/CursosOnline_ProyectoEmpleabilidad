using CoursesOnline.Domain.Entities;

namespace CoursesOnline.Domain.Interfaces;

public interface ILessonRepository
{
    Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId);
    Task<bool> ExistsWithOrderAsync(Guid courseId, int order);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task<Lesson?> GetByIdAsync(Guid lessonId);
}