using HotelListing.Data;
using HotelListing.Services;
using HotelListing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesService _service;

        public CountriesController(ICountriesService service)
        {
            _service = service;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return Ok(await _service.RetrieveAll());
        }

        // GET: api/Countries/5
        [HttpGet("{id:int}", Name = "GetCountry")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            Country foundCountry = await _service.RetrieveById(id);
            if ( foundCountry == null)
            {
                return NotFound();
            }
            return Ok(foundCountry);
        }


        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country countryToAdd)
        {
            Country newlyAddedCountry = await _service.Create(countryToAdd);
            return CreatedAtAction("GetCountry", new { id = newlyAddedCountry.Id }, newlyAddedCountry);

        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest($"Record identifiers '{id}' and '{country.Id}' don't match.");
            }

            try
            {
                Country updatedCountry = await _service.Update(country);
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine("[CountriesController][PutCountry] =>");
                Console.WriteLine(e.StackTrace);
                Console.WriteLine();
                return Problem();
            }
        }
    }
}
