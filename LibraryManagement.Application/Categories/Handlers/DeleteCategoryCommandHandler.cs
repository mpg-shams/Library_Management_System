using Library_Management_System.LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.Categories.Commands.CategoryCommand;
using LibraryManagement.Application.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Categories.Commands.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _repository;
        private readonly LibraryDbContext _context;

        public DeleteCategoryCommandHandler(ICategoryRepository repository, LibraryDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id);
            if (category == null || category.Books.Any()) return false;

            _repository.Delete(request.Id);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
