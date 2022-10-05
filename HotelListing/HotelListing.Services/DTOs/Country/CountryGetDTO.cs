using HotelListing.Services.DTOs.Hotel;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Services.DTOs.Country
{
    public class CountryGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string ShortName { get; set; } = String.Empty;
        public IList<HotelDTO> Hotels { get; set; } = new List<HotelDTO>();
    }
}
