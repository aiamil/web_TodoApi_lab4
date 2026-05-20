namespace TodoApi.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Year { get; set; }
        public string? ImageUrl { get; set; }
        public string ArchitectName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public double AverageRating { get; set; }  // Средний рейтинг
    }

    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Year { get; set; }
        public string? ImageUrl { get; set; }
        public int ArchitectId { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Year { get; set; }
        public string? ImageUrl { get; set; }
        public int ArchitectId { get; set; }
        public int CategoryId { get; set; }
    }
}