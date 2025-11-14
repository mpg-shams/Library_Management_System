using Library_Management_System.LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.IRepositories;
using LibraryManagement.Infrastructure.Interfaces;
using LibraryManagement.Infrastructure.Repositories;

namespace LibraryManagement.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IAuthorRepository Authors { get; private set; }
        public IBookRepository Books { get; private set; }
        public IBorrowRecordRepository BorrowRecords { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;

            Authors = new AuthorRepository(context);
            Books = new BookRepository(context);
            BorrowRecords = new BorrowRecordRepository(context);
            Categories = new CategoryRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}