using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // "Жилой", "Коммерческий", "Общественный"

        public string? Description { get; set; }

        // Связь с проектами
        public ICollection<Project>? Projects { get; set; }
    }
}