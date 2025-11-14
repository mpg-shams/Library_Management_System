using Library_Management_System.LibraryManagement.Core.Entities;
using Library_Management_System.LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        public BookRepository(LibraryDbContext context) => _context = context;

        public async Task<IEnumerable<Book>> GetAllAsync()
            => await _context.Books.Include(b => b.Author).Include(b => b.Categories).ToListAsync();

        public async Task<Book?> GetByIdAsync(int id)
            => await _context.Books.Include(b => b.Author).Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}