using CoursesOnline.Domain.Entities;
using CoursesOnline.Domain.Enums;

namespace CoursesOnline.Domain.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task<IEnumerable<Course>> SearchAsync(string? query, CourseStatus? status, int page, int pageSize);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
}