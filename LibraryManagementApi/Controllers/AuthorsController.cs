using Library_Management_System.LibraryManagement.Application.DTOs;
using LibraryManagement.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _service;

        public AuthorsController(IAuthorService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> Get()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> Get(int id)
        {
            var author = await _service.GetByIdAsync(id);
            return author == null ? NotFound() : Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> Post([FromBody] AuthorDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AuthorDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}