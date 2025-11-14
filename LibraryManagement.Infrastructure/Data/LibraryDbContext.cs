using Library_Management_System.LibraryManagement.Core.Entities;
using Library_Management_System.LibraryManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.LibraryManagement.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<BorrowRecord> BorrowRecords { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .UsingEntity(j => j.ToTable("BookCategories"));
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);
            SeedData(modelBuilder);

        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            var author1 = new Author { Id = 1, Name = "Edward", BirthDate = new DateTime(1925, 12, 12), IsActive = true };
            var author2 = new Author { Id = 2, Name = "George", BirthDate = new DateTime(1935, 1, 5), IsActive = true };

            var category1 = new Category { Id = 1, Name = "Poet" };
            var category2 = new Category { Id = 2, Name = "Romans" };
            var category3 = new Category { Id = 3, Name = "Sientice" };

            modelBuilder.Entity<Author>().HasData(author1, author2);
            modelBuilder.Entity<Category>().HasData(category1, category2, category3);

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Fresh Air",
                    ISBN = "978-600-1234567",
                    PublishedDate = new DateTime(1957, 1, 1),
                    Price = 45000m,
                    Quantity = 5,
                    IsAvailable = true,
                    Status = BookStatus.Available,
                    AuthorId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "business",
                    ISBN = "978-600-7654321",
                    PublishedDate = new DateTime(1970, 1, 1),
                    Price = 38000m,
                    Quantity = 3,
                    IsAvailable = false,
                    Status = BookStatus.Borrowed,
                    AuthorId = 2
                }
            );

            modelBuilder.Entity<BorrowRecord>().HasData(
                new BorrowRecord
                {
                    Id = 1,
                    UserId = 101,
                    BookId = 2,
                    BorrowDate = DateTime.Now.AddDays(-10),
                    Status = BorrowStatus.Active
                }
            );
        }
    }
}