using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repository;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepositry _countryRepositry;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepositry countryRepositry, IMapper mapper)
        {
            _mapper = mapper;
            _countryRepositry = countryRepositry;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDTO>>(_countryRepositry.GetCountryList());
            if (!ModelState.IsValid)
                return BadRequest();

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

            var country = _mapper.Map<CountryDTO>(_countryRepositry.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDTO>(_countryRepositry.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }
    }
}
