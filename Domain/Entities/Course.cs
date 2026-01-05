using System.ComponentModel.DataAnnotations;
using CoursesOnline.Domain.Enums;

namespace CoursesOnline.Domain.Entities
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public CourseStatus Status { get; set; } = CourseStatus.Draft;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relación
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        /* =======================
           Reglas de Negocio
           ======================= */

        public void Publish()
        {
            if (!Lessons.Any(l => !l.IsDeleted))
                throw new InvalidOperationException(
                    "No se puede publicar un curso sin lecciones activas."
                );

            Status = CourseStatus.Published;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            Status = CourseStatus.Draft;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}