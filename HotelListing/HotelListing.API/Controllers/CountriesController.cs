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

        public CountriesController()
        {
            _service = new CountriesService();
        }

        //public CountriesController(ICountriesService service)
        //{
        //    _service = service;
        //}

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
            return Ok(await _service.RetrieveById(id));
        }


        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country countryToAdd)
        {
            Country newlyAddedCountry = await _service.Create(countryToAdd);
            return CreatedAtAction("GetCountry", new { id = newlyAddedCountry.Id }, newlyAddedCountry);

        }

    }
}
