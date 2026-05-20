using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public int Year { get; set; }

        public string? ImageUrl { get; set; }

        // Внешние ключи
        public int ArchitectId { get; set; }
        public int CategoryId { get; set; }

        // Навигационные свойства
        public Architect? Architect { get; set; }
        public Category? Category { get; set; }

        // Связь с отзывами
        public ICollection<Review>? Reviews { get; set; }
    }
}