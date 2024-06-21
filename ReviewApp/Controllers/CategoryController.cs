using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepositry;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepositry, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepositry = categoryRepositry;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepositry.GetCategories());
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(int categoryId)
        {
            if (!_categoryRepositry.CategoryExists(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDTO>(_categoryRepositry.GetCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByCategory(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_categoryRepositry.GetPokemonsByCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO category)
        {
            if (category == null)
                return BadRequest(ModelState);

            var categoryExists = _categoryRepositry.GetCategories()
                .Where(x => x.Name.Trim().ToLower() == category.Name.Trim().ToLower())
                .FirstOrDefault();

            if (categoryExists != null)
            {
                ModelState.AddModelError("", "Category already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(category);
            if (!_categoryRepositry.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created....");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO category)
        {
            if (category is null)
                return BadRequest(ModelState);

            if (categoryId != category.Id)
                return BadRequest(ModelState);

            if (!_categoryRepositry.CategoryExists(categoryId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(category);

            if(!_categoryRepositry.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepositry.CategoryExists(categoryId))
                return NotFound();

            var categoryDelete = _categoryRepositry.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepositry.DeleteCategory(categoryDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
