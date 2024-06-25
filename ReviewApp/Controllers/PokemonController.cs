using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReviewApp.DTO;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public PokemonController(
            IPokemonRepository pokemonRepository,
            IOwnerRepository ownerRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemons()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var pokemons = _mapper
                .Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());

            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemon = _mapper
                .Map<PokemonDTO>(_pokemonRepository.GetPokemon(id));

            return Ok(pokemon);
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rating = _pokemonRepository.GetPokemonRating(id);

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult CreatePokemon([FromQuery]int ownerId,[FromQuery] int categoryId,[FromBody] PokemonDTO pokemon)
        {
            if (pokemon is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonExists = _pokemonRepository.GetPokemons()
                .Any(x => x.Name.Trim().Equals(pokemon.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (pokemonExists)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId) ||
                !_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemon);

            try
            {
                if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving.");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", $"Database Update Exception: {ex.InnerException?.Message ?? ex.Message}");
                return StatusCode(500, ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokemonId, [FromQuery] int ownerId,
            [FromQuery] int categoryId, [FromBody] PokemonDTO pokemon)
        {
            if (pokemon is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokemonId) ||
                !_ownerRepository.OwnerExists(ownerId) ||
                !_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound(ModelState);
            }

            if (pokemonId != pokemon.Id)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemon);
            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            var pokemonDelete = _pokemonRepository.GetPokemon(pokemonId);

            if (!_pokemonRepository.DeletePokemon(pokemonDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
