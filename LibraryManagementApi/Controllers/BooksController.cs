using Library_Management_System.LibraryManagement.Application.DTOs;
using LibraryManagement.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> Get()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> Get(int id)
        {
            var book = await _service.GetByIdAsync(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> Post([FromBody] BookDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BookDto dto)
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