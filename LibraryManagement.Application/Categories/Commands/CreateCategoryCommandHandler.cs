using Library_Management_System.LibraryManagement.Core.Entities;
using LibraryManagement.Application.Interfaces;
using MediatR;

namespace LibraryManagement.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler
        : IRequestHandler<CreateCategoryCommand, Category>
    {
        private readonly ICategoryRepository _repository;

        public CreateCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Category> Handle(
            CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Category name is required.");

            if (await _repository.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException(
                    "A category with this name already exists.");

            var category = new Category
            {
                Name = request.Name
            };

            _repository.Add(category);
            await _repository.SaveChangesAsync();

            return category;
        }
    }
}
