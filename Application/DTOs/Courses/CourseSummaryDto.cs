using CoursesOnline.Domain.Enums;

namespace CoursesOnline.Application.DTOs
{
    public class CourseSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public CourseStatus Status { get; set; }
        public int TotalLessons { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}