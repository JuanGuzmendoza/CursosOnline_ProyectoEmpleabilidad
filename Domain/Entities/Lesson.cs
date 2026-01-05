using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesOnline.Domain.Entities
{
    public class Lesson
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CourseId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int Order { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navegación
        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; }

        /* =======================
           Reglas de Negocio
           ======================= */

        public void Update(string title, int order)
        {
            Title = title;
            Order = order;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeOrder(int newOrder)
        {
            Order = newOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}