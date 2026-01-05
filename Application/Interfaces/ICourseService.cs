using CoursesOnline.Application.DTOs;
using CoursesOnline.Domain.Enums;

namespace CoursesOnline.Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> SearchAsync(
            string? query,
            CourseStatus? status,
            int page,
            int pageSize);

        Task<CourseSummaryDto?> GetSummaryAsync(Guid courseId);

        Task<CourseDto> CreateAsync(CreateCourseDto dto);
        Task UpdateAsync(Guid courseId, UpdateCourseDto dto);

        Task PublishAsync(Guid courseId);
        Task UnpublishAsync(Guid courseId);
        Task DeleteAsync(Guid courseId);
    }
}