using Library_Management_System.LibraryManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.LibraryManagement.Core.Entities
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        public DateTime PublishedDate { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool IsAvailable { get; set; } = true;

        public BookStatus Status { get; set; } = BookStatus.Available;

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public List<Category> Categories { get; set; } = new();

        public List<BorrowRecord> BorrowRecords { get; set; } = new();
    }
}