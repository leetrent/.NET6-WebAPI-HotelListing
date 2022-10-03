using HotelListing.Services.DTOs.Country;
using HotelListing.Services.DTOs.Hotel;
using HotelListing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsService _service;
        public HotelsController(IHotelsService service)
        {
            _service = service;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelGetDTO>>> GetHotels()
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

        // GET: api/Hotels/5
        [HttpGet("{id:int}", Name = "GetHotel")]
        public async Task<ActionResult<HotelGetDTO>> GetHotel(int id)
        {
            try
            {
                HotelGetDTO foundHotel = await _service.RetrieveById(id);
                if (foundHotel == null)
                {
                    return NotFound();
                }
                return Ok(foundHotel);
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
        public async Task<ActionResult<HotelGetDTO>> PostHotel(HotelCreateDTO hotelToAdd)
        {
            try
            {
                HotelGetDTO newlyAddedHotel = await _service.Create(hotelToAdd);
                return CreatedAtAction("GetHotel", new { id = newlyAddedHotel.Id }, newlyAddedHotel);
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

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelUpdateDTO hotelToUpdate)
        {
            try
            {
                if (id != hotelToUpdate.Id)
                {
                    return BadRequest($"Record identifiers '{id}' and '{hotelToUpdate.Id}' don't match.");
                }

                HotelGetDTO updatedHotel = await _service.Update(hotelToUpdate);
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
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
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
