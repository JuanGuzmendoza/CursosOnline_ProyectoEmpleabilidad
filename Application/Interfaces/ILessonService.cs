using CoursesOnline.Application.DTOs;

namespace CoursesOnline.Application.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDto>> GetByCourseAsync(Guid courseId);

        Task<LessonDto> CreateAsync(CreateLessonDto dto);
        Task UpdateAsync(Guid lessonId, UpdateLessonDto dto);

        Task DeleteAsync(Guid lessonId);
        Task ReorderAsync(Guid lessonId, int newOrder);
    }
}