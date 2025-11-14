using Library_Management_System.LibraryManagement.Application.DTOs;
using Library_Management_System.LibraryManagement.Core.Entities;
using LibraryManagement.Application.IServices;
using LibraryManagement.Infrastructure.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Category name is required.");

            if (await _unitOfWork.Categories.ExistsByNameAsync(dto.Name))
                throw new InvalidOperationException("A category with this name already exists.");

            var category = new Category
            {
                Name = dto.Name
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = category.Id;
            return dto;
        }

        public async Task UpdateAsync(int id, CategoryDto dto)
        {
            if (id != dto.Id)
                throw new ArgumentException("Category ID in the request does not match the provided ID.");

            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Category name is required.");

            if (category.Name != dto.Name && await _unitOfWork.Categories.ExistsByNameAsync(dto.Name))
                throw new InvalidOperationException("The new name is already in use.");

            category.Name = dto.Name;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            if (category.Books.Any())
                throw new InvalidOperationException("Cannot delete a category that contains books.");

            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}