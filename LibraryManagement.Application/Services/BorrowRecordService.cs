using Library_Management_System.LibraryManagement.Application.DTOs;
using Library_Management_System.LibraryManagement.Core.Entities;
using LibraryManagement.Application.IServices;
using LibraryManagement.Infrastructure.Interfaces;
using Library_Management_System.LibraryManagement.Core.Enums;

namespace LibraryManagement.Application.Services
{
    public class BorrowRecordService : IBorrowRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BorrowRecordDto>> GetAllAsync()
        {
            var records = await _unitOfWork.BorrowRecords.GetAllAsync();
            return records.Select(r => new BorrowRecordDto
            {
                Id = r.Id,
                UserId = r.UserId,
                BookId = r.BookId,
                BookTitle = r.Book.Title,
                AuthorName = r.Book.Author.Name,
                BorrowDate = r.BorrowDate,
                ReturnDate = r.ReturnDate,
                Status = r.Status,
                IsOverdue = r.IsOverdue
            });
        }

        public async Task<BorrowRecordDto?> GetByIdAsync(int id)
        {
            var record = await _unitOfWork.BorrowRecords.GetByIdAsync(id);
            if (record == null) return null;

            return new BorrowRecordDto
            {
                Id = record.Id,
                UserId = record.UserId,
                BookId = record.BookId,
                BookTitle = record.Book.Title,
                AuthorName = record.Book.Author.Name,
                BorrowDate = record.BorrowDate,
                ReturnDate = record.ReturnDate,
                Status = record.Status,
                IsOverdue = record.IsOverdue
            };
        }

        public async Task<BorrowRecordDto> BorrowBookAsync(BorrowRecordDto dto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(dto.BookId);
            if (book == null)
                throw new KeyNotFoundException("Book not found.");

            if (!book.IsAvailable)
                throw new InvalidOperationException("The book is currently not available.");

            book.IsAvailable = false;
            book.Status = BookStatus.Borrowed;

            var record = new BorrowRecord
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                BorrowDate = DateTime.Now,
                Status = BorrowStatus.Active
            };

            await _unitOfWork.BorrowRecords.AddAsync(record);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = record.Id;
            dto.BookTitle = book.Title;
            dto.AuthorName = book.Author.Name;
            dto.BorrowDate = record.BorrowDate;
            dto.Status = record.Status;
            dto.IsOverdue = false;

            return dto;
        }

        public async Task ReturnBookAsync(int id, DateTime? returnDate = null)
        {
            var record = await _unitOfWork.BorrowRecords.GetByIdAsync(id);
            if (record == null)
                throw new KeyNotFoundException("Borrow record not found.");

            if (record.Status != BorrowStatus.Active)
                throw new InvalidOperationException("This borrow has already been returned.");

            var book = await _unitOfWork.Books.GetByIdAsync(record.BookId);
            if (book == null)
                throw new KeyNotFoundException("Book not found.");

            record.ReturnDate = returnDate ?? DateTime.Now;
            record.Status = BorrowStatus.Returned;
            book.IsAvailable = true;
            book.Status = BookStatus.Available;

            await _unitOfWork.BorrowRecords.UpdateAsync(record);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _unitOfWork.BorrowRecords.GetByIdAsync(id);
            if (record == null)
                throw new KeyNotFoundException("Borrow record not found.");

            if (record.Status == BorrowStatus.Active)
            {
                var book = await _unitOfWork.Books.GetByIdAsync(record.BookId);
                if (book != null)
                {
                    book.IsAvailable = true;
                    book.Status = BookStatus.Available;
                }
            }

            await _unitOfWork.BorrowRecords.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}