using LibraryManagement.Application.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler
        : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _repository;

        public DeleteCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id);

            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");

            if (category.Books != null && category.Books.Any())
                throw new InvalidOperationException(
                    "Cannot delete a category that contains books.");

            _repository.Delete(category);
            await _repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
