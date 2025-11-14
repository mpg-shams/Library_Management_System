using System;
using Library_Management_System.LibraryManagement.Core.Enums;

namespace Library_Management_System.LibraryManagement.Application.DTOs
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = null!; 
        public string AuthorName { get; set; } = null!;

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowStatus Status { get; set; }
        public bool IsOverdue { get; set; }
    }
}