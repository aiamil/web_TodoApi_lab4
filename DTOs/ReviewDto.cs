namespace TodoApi.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string? Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProjectName { get; set; } = string.Empty;
    }

    public class CreateReviewDto
    {
        public string ClientName { get; set; } = string.Empty;
        public string? Text { get; set; }
        public int Rating { get; set; }
        public int ProjectId { get; set; }
    }

    public class UpdateReviewDto
    {
        public string ClientName { get; set; } = string.Empty;
        public string? Text { get; set; }
        public int Rating { get; set; }
    }
}