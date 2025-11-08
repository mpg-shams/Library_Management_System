using Library_Management_System.LibraryManagement.Core.Entities;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBorrowRecordRepository
    {
        Task<IEnumerable<BorrowRecord>> GetAllAsync();
        Task<BorrowRecord?> GetByIdAsync(int id);
        Task AddAsync(BorrowRecord record);
        Task UpdateAsync(BorrowRecord record);
        Task DeleteAsync(int id);
        Task<Book?> GetBookAsync(int bookId);
    }
}