using HotelListing.Services.DTOs.Country;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Services.DTOs.Hotel
{
    public class HotelGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public double Rating { get; set; }

        public CountryDTO? Country { get; set; }
    }
}
