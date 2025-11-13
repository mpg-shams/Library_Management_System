using Library_Management_System.LibraryManagement.Core.Entities;
using MediatR;

namespace LibraryManagement.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<Category?>
    {
        public int Id { get; set; }
    }
}
