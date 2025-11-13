using MediatR;

namespace LibraryManagement.Application.Categories.Commands.CategoryCommand
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
