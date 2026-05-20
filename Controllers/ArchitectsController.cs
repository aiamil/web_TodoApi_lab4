using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchitectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArchitectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/architects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArchitectDto>>> GetAll()
        {
            var architects = await _unitOfWork.Architects.GetAllAsync();

            var result = new List<ArchitectDto>();
            foreach (var a in architects)
            {
                var projects = await _unitOfWork.Projects.FindAsync(p => p.ArchitectId == a.Id);
                result.Add(new ArchitectDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Bio = a.Bio,
                    Specialization = a.Specialization,
                    PhotoUrl = a.PhotoUrl,
                    ProjectsCount = projects.Count()
                });
            }

            return Ok(result);
        }

        // GET: api/architects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArchitectDto>> GetById(int id)
        {
            var architect = await _unitOfWork.Architects.GetByIdAsync(id);
            if (architect == null) return NotFound();

            var projects = await _unitOfWork.Projects.FindAsync(p => p.ArchitectId == architect.Id);

            return Ok(new ArchitectDto
            {
                Id = architect.Id,
                Name = architect.Name,
                Bio = architect.Bio,
                Specialization = architect.Specialization,
                PhotoUrl = architect.PhotoUrl,
                ProjectsCount = projects.Count()
            });
        }

        // POST: api/architects
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ArchitectDto>> Create(CreateArchitectDto dto)
        {
            var architect = new Architect
            {
                Name = dto.Name,
                Bio = dto.Bio,
                Specialization = dto.Specialization,
                PhotoUrl = dto.PhotoUrl
            };

            await _unitOfWork.Architects.AddAsync(architect);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetById), new { id = architect.Id }, new ArchitectDto
            {
                Id = architect.Id,
                Name = architect.Name,
                Bio = architect.Bio,
                Specialization = architect.Specialization,
                PhotoUrl = architect.PhotoUrl,
                ProjectsCount = 0
            });
        }

        // PUT: api/architects/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateArchitectDto dto)
        {
            var architect = await _unitOfWork.Architects.GetByIdAsync(id);
            if (architect == null) return NotFound();

            architect.Name = dto.Name;
            architect.Bio = dto.Bio;
            architect.Specialization = dto.Specialization;
            architect.PhotoUrl = dto.PhotoUrl;

            _unitOfWork.Architects.Update(architect);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/architects/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var architect = await _unitOfWork.Architects.GetByIdAsync(id);
            if (architect == null) return NotFound();

            _unitOfWork.Architects.Delete(architect);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}