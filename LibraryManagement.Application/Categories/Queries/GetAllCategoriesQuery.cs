using Library_Management_System.LibraryManagement.Core.Entities;
using MediatR;

namespace LibraryManagement.Application.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>>
    {
    }
}
