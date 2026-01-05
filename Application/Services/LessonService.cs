using CoursesOnline.Application.DTOs;
using CoursesOnline.Application.Interfaces;
using CoursesOnline.Domain.Entities;
using CoursesOnline.Domain.Interfaces;

namespace CoursesOnline.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;

        public LessonService(
            ILessonRepository lessonRepository,
            ICourseRepository courseRepository)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<LessonDto>> GetByCourseAsync(Guid courseId)
        {
            var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);

            return lessons.Select(l => new LessonDto
            {
                Id = l.Id,
                Title = l.Title,
                Order = l.Order
            });
        }

        public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
        {
            if (await _lessonRepository.ExistsWithOrderAsync(dto.CourseId, dto.Order))
                throw new InvalidOperationException("El orden ya existe en el curso");

            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                CourseId = dto.CourseId,
                Title = dto.Title,
                Order = dto.Order,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _lessonRepository.AddAsync(lesson);

            return new LessonDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Order = lesson.Order
            };
        }

        public async Task UpdateAsync(Guid lessonId, UpdateLessonDto dto)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                         ?? throw new KeyNotFoundException("Lección no encontrada");

            if (lesson.Order != dto.Order &&
                await _lessonRepository.ExistsWithOrderAsync(lesson.CourseId, dto.Order))
                throw new InvalidOperationException("Orden duplicado");

            lesson.Update(dto.Title, dto.Order);
            await _lessonRepository.UpdateAsync(lesson);
        }

        public async Task DeleteAsync(Guid lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                         ?? throw new KeyNotFoundException("Lección no encontrada");

            lesson.SoftDelete();
            await _lessonRepository.UpdateAsync(lesson);
        }

        public async Task ReorderAsync(Guid lessonId, int newOrder)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                         ?? throw new KeyNotFoundException("Lección no encontrada");

            if (await _lessonRepository.ExistsWithOrderAsync(lesson.CourseId, newOrder))
                throw new InvalidOperationException("Orden duplicado");

            lesson.ChangeOrder(newOrder);
            await _lessonRepository.UpdateAsync(lesson);
        }
    }
}
