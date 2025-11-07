using Library_Management_System.LibraryManagement.Core.Entities;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _repo;

        public AuthorsController(IAuthorRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> Get()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await _repo.GetByIdAsync(id);
            return author == null ? NotFound() : Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<Author>> Post([FromBody] Author author)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrWhiteSpace(author.Name))
                return BadRequest("Author name is required.");

            author.IsActive = true;
            await _repo.AddAsync(author);
            return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Author author)
        {
            if (id != author.Id || !ModelState.IsValid) return BadRequest();
            if (string.IsNullOrWhiteSpace(author.Name))
                return BadRequest("Author name is required.");

            await _repo.UpdateAsync(author);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _repo.GetByIdAsync(id);
            if (author == null) return NotFound();
            if (author.Books.Any())
                return BadRequest("Cannot delete author with existing books.");

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}