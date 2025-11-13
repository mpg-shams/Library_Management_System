using Library_Management_System.LibraryManagement.Core.Entities;
using MediatR;

namespace LibraryManagement.Application.Categories.Queries.CategoriesQueries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>>
    {
    }
}
