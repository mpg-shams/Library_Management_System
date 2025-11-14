using Library_Management_System.LibraryManagement.Core.Entities;
using Library_Management_System.LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.IRepositories;
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

        public Task AddAsync(BorrowRecord borrowRecord)
        {
            _context.BorrowRecords.Add(borrowRecord);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(BorrowRecord borrowRecord)
        {
            _context.BorrowRecords.Update(borrowRecord);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.BorrowRecords.FindAsync(id);
            if (record != null)
            {
                _context.BorrowRecords.Remove(record);
            }
        }

        public async Task<Book?> GetBookAsync(int bookId)
            => await _context.Books.FindAsync(bookId);
    }
}