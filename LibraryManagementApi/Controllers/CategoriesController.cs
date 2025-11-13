using LibraryManagement.Application.Categories.Commands.CategoryCommand;
using LibraryManagement.Application.Categories.Queries.CategoriesQueries;
using LibraryManagement.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICategoryRepository _repository;

        public CategoriesController(IMediator mediator, ICategoryRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrWhiteSpace(command.Name))
                return BadRequest("Category name is required.");

            if (await _repository.ExistsByNameAsync(command.Name))
                return Conflict("A category with this name already exists.");

            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, new { id, command.Name });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id || !ModelState.IsValid) return BadRequest();
            if (string.IsNullOrWhiteSpace(command.Name))
                return BadRequest("Category name is required.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (existing.Name != command.Name && await _repository.ExistsByNameAsync(command.Name))
                return Conflict("The new name is already taken.");

            var success = await _mediator.Send(command);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCategoryCommand { Id = id };
            var success = await _mediator.Send(command);
            return success ? NoContent() : BadRequest("Cannot delete category or category not found.");
        }
    }
}
