using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewApp.DTO;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepositry;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        public CountryController(
            ICountryRepository countryRepositry,
            IOwnerRepository ownerRepository,
            IMapper mapper)
        {
            _countryRepositry = countryRepositry;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountries()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var countries = _mapper
                .Map<List<CountryDTO>>(_countryRepositry.GetCountries());

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepositry.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = _mapper
                .Map<CountryDTO>(_countryRepositry.GetCountry(countryId));

            return Ok(country);
        }

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = _mapper
                .Map<CountryDTO>(_countryRepositry.GetCountryByOwner(ownerId));

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDTO country)
        {
            if (country is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var countryExists = _countryRepositry.GetCountries()
                .Any(x => x.Name.Trim().Equals(country.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (countryExists)
            {
                ModelState.AddModelError("", "Country already exists.");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(country);
            try
            {
                if (!_countryRepositry.CreateCountry(countryMap))
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

        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDTO country)
        {
            if (country is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (countryId != country.Id)
                return BadRequest(ModelState);

            if (!_countryRepositry.CountryExists(countryId))
                return NotFound(ModelState);

            var countryMap = _mapper.Map<Country>(country);

            if (!_countryRepositry.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepositry.CountryExists(countryId))
                return NotFound();

            var countryDelete = _countryRepositry.GetCountry(countryId);

            if (!_countryRepositry.DeleteCountry(countryDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
