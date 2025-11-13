using MediatR;

namespace LibraryManagement.Application.Categories.Commands.CategoryCommand
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
