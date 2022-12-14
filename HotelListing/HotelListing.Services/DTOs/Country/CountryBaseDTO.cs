using System.ComponentModel.DataAnnotations;

namespace HotelListing.Services.DTOs.Country
{
    public class CountryBaseDTO
    {
        [Required]
        [Display(Name = "Country Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z'','\s]{1,40}$", ErrorMessage = "Only upper case characters, lower case characters and the comma character are allowed.")]
        public string Name { get; set; } = String.Empty;


        [Required]
        [Display(Name = "Country Abbreviation")]
        [StringLength(3, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [RegularExpression(@"^[A-Z'\s]{1,40}$", ErrorMessage = "Only upper case characters are allowed.")]
        public string ShortName { get; set; } = String.Empty;
    }
}
