using Library_Management_System.LibraryManagement.Core.Entities;
using Library_Management_System.LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;
        public AuthorRepository(LibraryDbContext context) => _context = context;

        public async Task<IEnumerable<Author>> GetAllAsync()
            => await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();

        public async Task<Author?> GetByIdAsync(int id)
            => await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

        public Task AddAsync(Author author)
        {
            _context.Authors.Add(author);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
        }
    }
}