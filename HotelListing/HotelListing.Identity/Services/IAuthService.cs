using HotelListing.Identity.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Identity.Services
{
    public interface IAuthService
    {
        Task<IEnumerable<IdentityError>> RegisterUser(UserDTO dto);

        Task<TokenDTO?> Login(LoginDTO loginDTO);
        Task<TokenDTO?> RefreshToken(TokenDTO oldToken);
    }
}
