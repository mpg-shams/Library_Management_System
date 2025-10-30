using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public bool IsActive { get; set; } = true;

        public List<Book> Books { get; set; } = new();
    }
}