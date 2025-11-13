using Library_Management_System.LibraryManagement.Core.Entities;
using Library_Management_System.LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.Categories.Commands.CategoryCommand;
using LibraryManagement.Application.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Categories.Commands.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository _repository;
        private readonly LibraryDbContext _context;

        public CreateCategoryCommandHandler(ICategoryRepository repository, LibraryDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category { Name = request.Name };
            _repository.Add(category);
            await _context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
