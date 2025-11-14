using Library_Management_System.LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.IServices
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAsync();
        Task<AuthorDto?> GetByIdAsync(int id);
        Task<AuthorDto> CreateAsync(AuthorDto dto);
        Task UpdateAsync(int id, AuthorDto dto);
        Task DeleteAsync(int id);
    }
}