using System.ComponentModel.DataAnnotations;

namespace HotelListing.Services.DTOs.Hotel
{
    public class HotelBaseDTO
    {
        [Required]
        [Display(Name = "Hotel Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z'','\s]{1,40}$", ErrorMessage = "Only upper case characters, lower case characters and the comma character are allowed.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Hotel City")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z'','\s]{1,40}$", ErrorMessage = "Only upper case characters, lower case characters and the comma character are allowed.")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Hotel Rating")]
        public double Rating { get; set; }

        [Required]
        [Display(Name = "Country ID")]
        public int CountryId { get; set; }
    }
}
