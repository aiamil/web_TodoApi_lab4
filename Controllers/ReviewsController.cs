using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();

            var result = new List<ReviewDto>();
            foreach (var r in reviews)
            {
                var project = await _unitOfWork.Projects.GetByIdAsync(r.ProjectId);
                result.Add(new ReviewDto
                {
                    Id = r.Id,
                    ClientName = r.ClientName,
                    Text = r.Text,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt,
                    ProjectName = project?.Name ?? ""
                });
            }

            return Ok(result);
        }

        // GET: api/reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetById(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null) return NotFound();

            var project = await _unitOfWork.Projects.GetByIdAsync(review.ProjectId);

            return Ok(new ReviewDto
            {
                Id = review.Id,
                ClientName = review.ClientName,
                Text = review.Text,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt,
                ProjectName = project?.Name ?? ""
            });
        }

        // GET: api/reviews/by-project/5
        [HttpGet("by-project/{projectId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByProjectId(int projectId)
        {
            var reviews = await _unitOfWork.Reviews.FindAsync(r => r.ProjectId == projectId);
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId);

            var result = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ClientName = r.ClientName,
                Text = r.Text,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt,
                ProjectName = project?.Name ?? ""
            });

            return Ok(result);
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create(CreateReviewDto dto)
        {
            // Проверяем, существует ли проект
            var project = await _unitOfWork.Projects.GetByIdAsync(dto.ProjectId);
            if (project == null)
                return BadRequest("Проект не найден");

            var review = new Review
            {
                ClientName = dto.ClientName,
                Text = dto.Text,
                Rating = dto.Rating,
                ProjectId = dto.ProjectId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetById), new { id = review.Id }, new ReviewDto
            {
                Id = review.Id,
                ClientName = review.ClientName,
                Text = review.Text,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt,
                ProjectName = project.Name
            });
        }

        // DELETE: api/reviews/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null) return NotFound();

            _unitOfWork.Reviews.Delete(review);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}