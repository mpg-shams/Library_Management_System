using MediatR;

namespace LibraryManagement.Application.Categories.Commands.CategoryCommand
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
    }
}
