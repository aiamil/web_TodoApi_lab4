namespace TodoApi.DTOs
{
    // Для GET запросов (возвращаем данные)
    public class ArchitectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Specialization { get; set; }
        public string? PhotoUrl { get; set; }
        public int ProjectsCount { get; set; }  // Количество проектов
    }

    // Для POST запросов (создание)
    public class CreateArchitectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Specialization { get; set; }
        public string? PhotoUrl { get; set; }
    }

    // Для PUT запросов (обновление)
    public class UpdateArchitectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Specialization { get; set; }
        public string? PhotoUrl { get; set; }
    }
}