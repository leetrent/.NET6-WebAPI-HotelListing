using System.ComponentModel.DataAnnotations;

namespace HotelListing.Identity.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // WILL SERVE AS USER NAME

        [Required]
        //[PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }
}