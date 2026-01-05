using System.ComponentModel.DataAnnotations;

namespace CoursesOnline.Application.DTOs
{
    public class UpdateCourseDto
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Title { get; set; } = string.Empty;
    }
}