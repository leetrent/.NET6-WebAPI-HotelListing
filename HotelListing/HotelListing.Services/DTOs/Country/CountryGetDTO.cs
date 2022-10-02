using HotelListing.Services.DTOs.Hotel;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Services.DTOs.Country
{
    public class CountryGetDTO : CountryBaseDTO
    {
        [Required]
        [Display(Name = "Country ID")]
        public int Id { get; set; }


        [Display(Name = "Hotels in Country")]
        public IList<HotelGetDTO> Hotels { get; set; } = new List<HotelGetDTO>();
    }
}
