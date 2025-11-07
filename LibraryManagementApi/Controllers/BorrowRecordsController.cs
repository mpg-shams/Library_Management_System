using Library_Management_System.LibraryManagement.Core.Entities;
using Library_Management_System.LibraryManagement.Core.Enums;
using Library_Management_System.LibraryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowRecordsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRecord>>> Get()
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .ThenInclude(b => b!.Author)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRecord>> Get(int id)
        {
            var record = await _context.BorrowRecords
                .Include(br => br.Book)
                .ThenInclude(b => b!.Author)
                .FirstOrDefaultAsync(br => br.Id == id);

            return record == null ? NotFound() : Ok(record);
        }

        [HttpPost]
        public async Task<ActionResult<BorrowRecord>> Post([FromBody] BorrowRecord record)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _context.Books.AnyAsync(b => b.Id == record.BookId))
                return BadRequest("Invalid BookId.");

            var book = await _context.Books.FindAsync(record.BookId);
            if (book == null || !book.IsAvailable)
                return BadRequest("Book is not available for borrowing.");

            book.IsAvailable = false;
            book.Status = BookStatus.Borrowed;

            record.BorrowDate = DateTime.Now;
            record.Status = BorrowStatus.Active;

            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();

            await _context.Entry(record).Reference(r => r.Book).LoadAsync();
            await _context.Entry(record.Book).Reference(b => b.Author).LoadAsync();

            return CreatedAtAction(nameof(Get), new { id = record.Id }, record);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BorrowRecord record)
        {
            if (id != record.Id || !ModelState.IsValid) return BadRequest();

            var existing = await _context.BorrowRecords.FindAsync(id);
            if (existing == null) return NotFound();

            var book = await _context.Books.FindAsync(record.BookId);
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

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.BorrowRecords.FindAsync(id);
            if (record == null) return NotFound();

            if (record.Status == BorrowStatus.Active)
            {
                var book = await _context.Books.FindAsync(record.BookId);
                if (book != null)
                {
                    book.IsAvailable = true;
                    book.Status = BookStatus.Available;
                }
            }

            _context.BorrowRecords.Remove(record);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}