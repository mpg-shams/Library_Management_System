using Library_Management_System.LibraryManagement.Core.Enums;

namespace Library_Management_System.LibraryManagement.Core.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public DateTime BorrowDate { get; set; } = DateTime.Now;

        public DateTime? ReturnDate { get; set; }

        public BorrowStatus Status { get; set; } = BorrowStatus.Active;

        public bool IsOverdue => ReturnDate == null && BorrowDate.AddDays(14) < DateTime.Now;
    }
}