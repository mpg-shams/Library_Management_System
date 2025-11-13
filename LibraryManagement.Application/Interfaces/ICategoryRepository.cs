using Library_Management_System.LibraryManagement.Core.Entities;

namespace LibraryManagement.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
