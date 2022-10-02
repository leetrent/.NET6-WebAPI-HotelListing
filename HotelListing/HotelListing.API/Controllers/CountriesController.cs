﻿using HotelListing.Services.DTOs.Country;
using HotelListing.Services.Interfaces;
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
                Console.WriteLine("[CountriesController][GetCountries] =>");
                Console.WriteLine(exc.Message);
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
                    return NotFound();
                }
                return Ok(foundCountry);
            }
            catch (Exception exc)
            {

                Console.WriteLine("[CountriesController][GetCountry] =>");
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CountryGetDTO>> PostCountry(CountryCreateDTO countryToAdd)
        {
            try
            {
                CountryGetDTO newlyAddedCountry = await _service.Create(countryToAdd);
                return CreatedAtAction("GetCountry", new { id = newlyAddedCountry.Id }, newlyAddedCountry);
            }
            catch (Exception exc)
            {
                Console.WriteLine("[CountriesController][PostCountry] =>");
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }

        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, CountryUpdateDTO countryToUpdate)
        {
            try
            {
                if (id != countryToUpdate.Id)
                {
                    return BadRequest($"Record identifiers '{id}' and '{countryToUpdate.Id}' don't match.");
                }

                CountryGetDTO updatedCountry = await _service.Update(countryToUpdate);
                return NoContent();
            }
            catch (Exception exc)
            {
                Console.WriteLine("[CountriesController][PutCountry] =>");
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            try
            {
                await _service.Delete(id);
                return NoContent();
            }
            catch (Exception exc)
            {
                Console.WriteLine("[CountriesController][DeleteCountry] =>");
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine();
                return Problem();
            }
        }
    }
}
