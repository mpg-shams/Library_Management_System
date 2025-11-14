using Library_Management_System.LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CategoryDto dto);
        Task UpdateAsync(int id, CategoryDto dto);
        Task DeleteAsync(int id);
    }
}