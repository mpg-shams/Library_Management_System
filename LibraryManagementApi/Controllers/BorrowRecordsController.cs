using Library_Management_System.LibraryManagement.Core.Entities;
using Library_Management_System.LibraryManagement.Core.Enums;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordRepository _repo;

        public BorrowRecordsController(IBorrowRecordRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRecord>>> Get()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRecord>> Get(int id)
        {
            var record = await _repo.GetByIdAsync(id);
            return record == null ? NotFound() : Ok(record);
        }

        [HttpPost]
        public async Task<ActionResult<BorrowRecord>> Post([FromBody] BorrowRecord record)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = await _repo.GetBookAsync(record.BookId);
            if (book == null) return BadRequest("Invalid BookId.");
            if (!book.IsAvailable) return BadRequest("Book is not available.");

            book.IsAvailable = false;
            book.Status = BookStatus.Borrowed;
            record.BorrowDate = DateTime.Now;
            record.Status = BorrowStatus.Active;

            await _repo.AddAsync(record);
            return CreatedAtAction(nameof(Get), new { id = record.Id }, record);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BorrowRecord record)
        {
            if (id != record.Id || !ModelState.IsValid) return BadRequest();

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var book = await _repo.GetBookAsync(record.BookId);
            if (book == null) return BadRequest("Invalid BookId.");

            existing.UserId = record.UserId;
            existing.BookId = record.BookId;
            existing.ReturnDate = record.ReturnDate;

            if (record.ReturnDate.HasValue)
            {
                existing.Status = BorrowStatus.Returned;
                book.IsAvailable = true;
                book.Status = BookStatus.Available;
            }
            else
            {
                existing.Status = BorrowStatus.Active;
            }

            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _repo.GetByIdAsync(id);
            if (record == null) return NotFound();

            if (record.Status == BorrowStatus.Active)
            {
                var book = await _repo.GetBookAsync(record.BookId);
                if (book != null)
                {
                    book.IsAvailable = true;
                    book.Status = BookStatus.Available;
                }
            }

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}