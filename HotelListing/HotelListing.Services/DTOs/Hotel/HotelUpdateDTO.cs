using System.ComponentModel.DataAnnotations;

namespace HotelListing.Services.DTOs.Hotel
{
    public class HotelUpdateDTO
    {
        [Display(Name = "Hotel ID")]
        public int Id { get; set; }
    }
}
