using System;

namespace Library_Management_System.LibraryManagement.Application.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
    }
}