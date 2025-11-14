using Library_Management_System.LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.IServices
{
    public interface IBorrowRecordService
    {
        Task<IEnumerable<BorrowRecordDto>> GetAllAsync();
        Task<BorrowRecordDto?> GetByIdAsync(int id);
        Task<BorrowRecordDto> BorrowBookAsync(BorrowRecordDto dto);
        Task ReturnBookAsync(int id, DateTime? returnDate = null);
        Task DeleteAsync(int id);
    }
}