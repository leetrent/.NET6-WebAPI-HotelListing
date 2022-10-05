using System.ComponentModel.DataAnnotations;

namespace HotelListing.Services.DTOs.Country
{
    public class CountryUpdateDTO : CountryBaseDTO
    {
        [Required]
        [Display(Name = "Country ID")]
        public int Id { get; set; }
    }
}
