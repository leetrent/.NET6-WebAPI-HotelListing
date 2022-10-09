using HotelListing.Identity.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Identity.Managers
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> RegisterUser(UserDTO userDTO);
        Task<bool> Login(LoginDTO loginDTO);
    }
}
