using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewApp.DTO;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repository;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(
            IOwnerRepository ownerRepository,
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners()
        {
            var owners = _mapper
                .Map<List<OwnerDTO>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var owner = _mapper
                .Map<OwnerDTO>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var owner = _mapper
                .Map<PokemonDTO>(_ownerRepository.GetPokemonsByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDTO owner)
        {
            if (owner == null)
                return BadRequest(ModelState);

            var ownerExists = _ownerRepository.GetOwners()
                .FirstOrDefault(x => x.Name.Trim().Equals(owner.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (ownerExists != null)
            {
                ModelState.AddModelError("", "Owner already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(owner);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            try
            {
                if (!_ownerRepository.CreateOwner(ownerMap))
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

            return Ok("Successfully created....");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDTO owner)
        {
            if (owner is null)
                return BadRequest(ModelState);

            if (ownerId != owner.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(owner);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
