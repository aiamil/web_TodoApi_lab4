using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ClientName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Text { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Внешний ключ
        public int ProjectId { get; set; }

        // Навигационное свойство
        public Project? Project { get; set; }
    }
}