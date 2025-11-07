using LibraryManagementApi.Data;
using LibraryManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public CategoriesController(LibraryDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
            => await _context.Categories
                .Include(c => c.Books)
                .ThenInclude(b => b.Author)
                .OrderBy(c => c.Name)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Books)
                .ThenInclude(b => b.Author)
                .FirstOrDefaultAsync(c => c.Id == id);

            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
                return Conflict("A category with this name already exists.");

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            if (id != category.Id || !ModelState.IsValid) return BadRequest();

            var existing = await _context.Categories.FindAsync(id);
            if (existing == null) return NotFound();

            if (existing.Name != category.Name &&
                await _context.Categories.AnyAsync(c => c.Name == category.Name))
                return Conflict("The new name is already taken.");

            existing.Name = category.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            if (await _context.Books.AnyAsync(b => b.Categories.Any(c => c.Id == id)))
                return BadRequest("Cannot delete a category that contains books.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}