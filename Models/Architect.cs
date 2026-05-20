using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Architect
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Bio { get; set; }

        [MaxLength(50)]
        public string? Specialization { get; set; }

        public string? PhotoUrl { get; set; }

        // Связь с проектами
        public ICollection<Project>? Projects { get; set; }
    }
}