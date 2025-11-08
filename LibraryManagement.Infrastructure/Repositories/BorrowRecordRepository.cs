using Library_Management_System.LibraryManagement.Core.Entities;
using Library_Management_System.LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly LibraryDbContext _context;
        public BorrowRecordRepository(LibraryDbContext context) => _context = context;

        public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
            => await _context.BorrowRecords
                .Include(br => br.Book)
                .ThenInclude(b => b!.Author)
                .ToListAsync();

        public async Task<BorrowRecord?> GetByIdAsync(int id)
            => await _context.BorrowRecords
                .Include(br => br.Book)
                .ThenInclude(b => b!.Author)
                .FirstOrDefaultAsync(br => br.Id == id);

        public async Task AddAsync(BorrowRecord record)
        {
            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BorrowRecord record)
        {
            _context.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.BorrowRecords.FindAsync(id);
            if (record != null)
            {
                _context.BorrowRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Book?> GetBookAsync(int bookId)
            => await _context.Books.FindAsync(bookId);
    }
}