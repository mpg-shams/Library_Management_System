using Library_Management_System.LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.IServices
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(int id);
        Task<BookDto> CreateAsync(BookDto dto);
        Task UpdateAsync(int id, BookDto dto);
        Task DeleteAsync(int id);
    }
}