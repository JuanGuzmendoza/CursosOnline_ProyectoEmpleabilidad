using CoursesOnline.Application.DTOs;
using CoursesOnline.Application.Interfaces;
using CoursesOnline.Domain.Entities;
using CoursesOnline.Domain.Enums;
using CoursesOnline.Domain.Interfaces;

namespace CoursesOnline.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseDto>> SearchAsync(
            string? query,
            CourseStatus? status,
            int page,
            int pageSize)
        {
            var courses = await _courseRepository
                .SearchAsync(query, status, page, pageSize);

            return courses.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });
        }

        public async Task<CourseSummaryDto?> GetSummaryAsync(Guid courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return null;

            return new CourseSummaryDto
            {
                Id = course.Id,
                Title = course.Title,
                Status = course.Status,
                TotalLessons = course.Lessons.Count(l => !l.IsDeleted),
                LastModifiedAt = course.UpdatedAt
            };
        }

        public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
        {
            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Status = CourseStatus.Draft,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _courseRepository.AddAsync(course);

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Status = course.Status,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };
        }

        public async Task UpdateAsync(Guid courseId, UpdateCourseDto dto)
        {
            var course = await _courseRepository.GetByIdAsync(courseId)
                ?? throw new KeyNotFoundException("Curso no encontrado");

            course.Title = dto.Title;
            course.UpdatedAt = DateTime.UtcNow;

            await _courseRepository.UpdateAsync(course);
        }

        public async Task PublishAsync(Guid courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId)
                ?? throw new KeyNotFoundException("Curso no encontrado");

            course.Publish();
            await _courseRepository.UpdateAsync(course);
        }

        public async Task UnpublishAsync(Guid courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId)
                ?? throw new KeyNotFoundException("Curso no encontrado");

            course.Unpublish();
            await _courseRepository.UpdateAsync(course);
        }

        public async Task DeleteAsync(Guid courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId)
                ?? throw new KeyNotFoundException("Curso no encontrado");

            course.SoftDelete();
            await _courseRepository.UpdateAsync(course);
        }
    }
}
