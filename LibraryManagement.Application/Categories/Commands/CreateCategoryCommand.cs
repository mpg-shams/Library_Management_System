using Library_Management_System.LibraryManagement.Core.Entities;
using MediatR;

namespace LibraryManagement.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Category>
    {
        public string Name { get; set; } = string.Empty;
    }
}
