using Library_Management_System.LibraryManagement.Application.DTOs;
using LibraryManagement.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordService _service;

        public BorrowRecordsController(IBorrowRecordService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> Get()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRecordDto>> Get(int id)
        {
            var record = await _service.GetByIdAsync(id);
            return record == null ? NotFound() : Ok(record);
        }

        [HttpPost]
        public async Task<ActionResult<BorrowRecordDto>> Post([FromBody] BorrowRecordDto dto)
        {
            var created = await _service.BorrowBookAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnBook(int id, [FromBody] DateTime? returnDate = null)
        {
            await _service.ReturnBookAsync(id, returnDate);
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