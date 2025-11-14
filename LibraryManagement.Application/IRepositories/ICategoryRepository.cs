using Library_Management_System.LibraryManagement.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagement.Application.IRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
