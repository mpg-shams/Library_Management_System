using Library_Management_System.LibraryManagement.Application.DTOs;
using Library_Management_System.LibraryManagement.Core.Entities;
using LibraryManagement.Application.IServices;
using LibraryManagement.Infrastructure.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();
            return authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name,
                BirthDate = a.BirthDate,
                IsActive = a.IsActive
            });
        }

        public async Task<AuthorDto?> GetByIdAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null) return null;

            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                BirthDate = author.BirthDate,
                IsActive = author.IsActive
            };
        }

        public async Task<AuthorDto> CreateAsync(AuthorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Author name is required.");

            var author = new Author
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                IsActive = dto.IsActive
            };

            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = author.Id;
            return dto;
        }

        public async Task UpdateAsync(int id, AuthorDto dto)
        {
            if (id != dto.Id)
                throw new ArgumentException("Author ID in the request does not match the provided ID.");

            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                throw new KeyNotFoundException("Author not found.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Author name is required.");

            author.Name = dto.Name;
            author.BirthDate = dto.BirthDate;
            author.IsActive = dto.IsActive;

            await _unitOfWork.Authors.UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                throw new KeyNotFoundException("Author not found.");

            if (author.Books.Any())
                throw new InvalidOperationException("Cannot delete an author who has associated books.");

            await _unitOfWork.Authors.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}