using System.ComponentModel.DataAnnotations;

namespace CoursesOnline.Application.DTOs
{
    public class CreateLessonDto
    {
        [Required(ErrorMessage = "El ID del curso es obligatorio")]
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "El orden es obligatorio")]
        public int Order { get; set; }
    }
}