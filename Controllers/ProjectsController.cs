using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/projects - доступно всем
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();

            var result = new List<ProjectDto>();
            foreach (var p in projects)
            {
                // Загружаем связанные данные
                var architect = await _unitOfWork.Architects.GetByIdAsync(p.ArchitectId);
                var category = await _unitOfWork.Categories.GetByIdAsync(p.CategoryId);

                result.Add(new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Year = p.Year,
                    ImageUrl = p.ImageUrl,
                    ArchitectName = architect?.Name ?? "",
                    CategoryName = category?.Name ?? "",
                    AverageRating = 0 // позже посчитаем из отзывов
                });
            }

            return Ok(result);
        }

        // GET: api/projects/5 - доступно всем
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null) return NotFound();

            var architect = await _unitOfWork.Architects.GetByIdAsync(project.ArchitectId);
            var category = await _unitOfWork.Categories.GetByIdAsync(project.CategoryId);

            return Ok(new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Year = project.Year,
                ImageUrl = project.ImageUrl,
                ArchitectName = architect?.Name ?? "",
                CategoryName = category?.Name ?? "",
                AverageRating = 0
            });
        }

        // POST: api/projects - только для авторизованных
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            // Проверяем, существует ли архитектор
            var architect = await _unitOfWork.Architects.GetByIdAsync(dto.ArchitectId);
            if (architect == null)
                return BadRequest("Архитектор не найден");

            // Проверяем, существует ли категория
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null)
                return BadRequest("Категория не найдена");

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                Year = dto.Year,
                ImageUrl = dto.ImageUrl,
                ArchitectId = dto.ArchitectId,
                CategoryId = dto.CategoryId
            };

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Year = project.Year,
                ImageUrl = project.ImageUrl,
                ArchitectName = architect.Name,
                CategoryName = category.Name
            });
        }

        // PUT: api/projects/5 - только для авторизованных
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null) return NotFound();

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.Year = dto.Year;
            project.ImageUrl = dto.ImageUrl;
            project.ArchitectId = dto.ArchitectId;
            project.CategoryId = dto.CategoryId;

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/projects/5 - только для авторизованных
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null) return NotFound();

            _unitOfWork.Projects.Delete(project);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}