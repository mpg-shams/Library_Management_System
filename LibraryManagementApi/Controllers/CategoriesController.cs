using Library_Management_System.LibraryManagement.Core.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repo;

        public CategoriesController(ICategoryRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrWhiteSpace(category.Name))
                return BadRequest("Category name is required.");

            if (await _repo.ExistsByNameAsync(category.Name))
                return Conflict("A category with this name already exists.");

            await _repo.AddAsync(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            if (id != category.Id || !ModelState.IsValid) return BadRequest();
            if (string.IsNullOrWhiteSpace(category.Name))
                return BadRequest("Category name is required.");

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (existing.Name != category.Name && await _repo.ExistsByNameAsync(category.Name))
                return Conflict("The new name is already taken.");

            existing.Name = category.Name;
            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return NotFound();
            if (category.Books.Any())
                return BadRequest("Cannot delete a category that contains books.");

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}