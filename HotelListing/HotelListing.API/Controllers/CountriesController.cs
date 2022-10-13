using HotelListing.Services.DTOs.Country;
using HotelListing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<CountryGetDTO>>> GetCountries()
        {
            try
            {
                return Ok(await _service.RetrieveAll());
            }
            catch (Exception exc)
            {
                Console.WriteLine();
                Console.WriteLine(exc.Message);
                Console.WriteLine();
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }
        }

        // GET: api/Countries/5
        [HttpGet("{id:int}", Name = "GetCountry")]
        public async Task<ActionResult<CountryGetDTO>> GetCountry(int id)
        {
            try
            {
                CountryGetDTO foundCountry = await _service.RetrieveById(id);
                if (foundCountry == null)
                {
                    return NotFound($"Country with an ID of '{id}' was not found.");
                }
                return Ok(foundCountry);
            }
            catch (Exception exc)
            {

                Console.WriteLine();
                Console.WriteLine(exc.Message);
                Console.WriteLine();
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<CountryGetDTO>> PostCountry(CountryCreateDTO countryToAdd)
        {
            try
            {
                CountryGetDTO newlyAddedCountry = await _service.Create(countryToAdd);
                return CreatedAtAction("GetCountry", new { id = newlyAddedCountry.Id }, newlyAddedCountry);
            }
            catch (Exception exc)
            {
                Console.WriteLine();
                Console.WriteLine(exc.Message);
                Console.WriteLine();
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }

        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutCountry(int id, CountryUpdateDTO countryToUpdate)
        {
            try
            {
                if (id != countryToUpdate.Id)
                {
                    return BadRequest($"Record identifiers '{id}' and '{countryToUpdate.Id}' don't match.");
                }

                if ( await _service.CountryExists(countryToUpdate.Id) == false)
                {
                    return NotFound($"Country with an ID '{countryToUpdate.Id}' was not found. Cannot update.");
                }

                CountryGetDTO updatedCountry = await _service.Update(countryToUpdate);
                return NoContent();
            }
            catch (Exception exc)
            {
                Console.WriteLine();
                Console.WriteLine(exc.Message);
                Console.WriteLine();
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize (Roles="Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            try
            {
                if (await _service.CountryExists(id) == false)
                {
                    return NotFound($"Country with an ID of '{id}' was not found. Cannot delete.");
                }

                if (await _service.CountryHasHotels(id))
                {
                    return ValidationProblem($"Country with an ID of '{id}' contains hotels. Cannot delete.");
                }

                await _service.Delete(id);
                return NoContent();
            }
            catch (Exception exc)
            {
                Console.WriteLine();
                Console.WriteLine(exc.Message);
                Console.WriteLine();
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }
        }
    }
}
