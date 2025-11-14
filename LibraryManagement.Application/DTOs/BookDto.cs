using System;
using Library_Management_System.LibraryManagement.Core.Enums;

namespace Library_Management_System.LibraryManagement.Application.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public BookStatus Status { get; set; }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!; 

        public List<int> CategoryIds { get; set; } = new();
        public List<string> CategoryNames { get; set; } = new();
    }
}