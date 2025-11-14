using LibraryManagement.Application.IRepositories;

namespace LibraryManagement.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository Authors { get; }
        IBookRepository Books { get; }
        IBorrowRecordRepository BorrowRecords { get; }
        ICategoryRepository Categories { get; }

        Task<int> SaveChangesAsync();
    }
}