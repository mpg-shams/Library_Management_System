using LibraryManagement.Application.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler
        : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _repository;

        public UpdateCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Category name is required.");

            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");

            if (existing.Name != request.Name &&
                await _repository.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException("The new name is already taken.");

            existing.Name = request.Name;
            _repository.Update(existing);
            await _repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
