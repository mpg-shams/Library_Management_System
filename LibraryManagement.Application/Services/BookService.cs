using Library_Management_System.LibraryManagement.Application.DTOs;
using Library_Management_System.LibraryManagement.Core.Entities;
using LibraryManagement.Application.IServices;
using LibraryManagement.Infrastructure.Interfaces;
using Library_Management_System.LibraryManagement.Core.Enums;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _unitOfWork.Books.GetAllAsync();
            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishedDate = b.PublishedDate,
                Price = b.Price,
                Quantity = b.Quantity,
                IsAvailable = b.IsAvailable,
                Status = b.Status,
                AuthorId = b.AuthorId,
                AuthorName = b.Author?.Name ?? "Unknown",
                CategoryIds = b.Categories.Select(c => c.Id).ToList(),
                CategoryNames = b.Categories.Select(c => c.Name).ToList()
            });
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null) return null;

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishedDate = book.PublishedDate,
                Price = book.Price,
                Quantity = book.Quantity,
                IsAvailable = book.IsAvailable,
                Status = book.Status,
                AuthorId = book.AuthorId,
                AuthorName = book.Author?.Name ?? "Unknown",
                CategoryIds = book.Categories.Select(c => c.Id).ToList(),
                CategoryNames = book.Categories.Select(c => c.Name).ToList()
            };
        }

        public async Task<BookDto> CreateAsync(BookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Book title is required.");

            if (string.IsNullOrWhiteSpace(dto.ISBN))
                throw new ArgumentException("ISBN is required.");

            var author = await _unitOfWork.Authors.GetByIdAsync(dto.AuthorId);
            if (author == null)
                throw new KeyNotFoundException("Author not found.");

            var book = new Book
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublishedDate = dto.PublishedDate,
                Price = dto.Price,
                Quantity = dto.Quantity,
                IsAvailable = dto.IsAvailable,
                Status = dto.Status,
                AuthorId = dto.AuthorId
            };

            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = book.Id;
            dto.AuthorName = author.Name;
            return dto;
        }

        public async Task UpdateAsync(int id, BookDto dto)
        {
            if (id != dto.Id)
                throw new ArgumentException("Book ID in the request does not match the provided ID.");

            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
                throw new KeyNotFoundException("Book not found.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Book title is required.");

            if (string.IsNullOrWhiteSpace(dto.ISBN))
                throw new ArgumentException("ISBN is required.");

            var author = await _unitOfWork.Authors.GetByIdAsync(dto.AuthorId);
            if (author == null)
                throw new KeyNotFoundException("Author not found.");

            book.Title = dto.Title;
            book.ISBN = dto.ISBN;
            book.PublishedDate = dto.PublishedDate;
            book.Price = dto.Price;
            book.Quantity = dto.Quantity;
            book.IsAvailable = dto.IsAvailable;
            book.Status = dto.Status;
            book.AuthorId = dto.AuthorId;

            await _unitOfWork.Books.UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
                throw new KeyNotFoundException("Book not found.");

            if (book.BorrowRecords.Any(br => br.Status == BorrowStatus.Active))
                throw new InvalidOperationException("Cannot delete a book that is currently borrowed.");

            await _unitOfWork.Books.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}