using Library_Management_System.LibraryManagement.Infrastructure.Data;
using LibraryManagement.Application.Categories.Commands.CategoryCommand;
using LibraryManagement.Application.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Categories.Commands.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepository _repository;
        private readonly LibraryDbContext _context;

        public UpdateCategoryCommandHandler(ICategoryRepository repository, LibraryDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id);
            if (category == null) return false;

            category.Name = request.Name;
            _repository.Update(category);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
