using AutoMapper;
using HotelListing.Identity.DTOs;
using HotelListing.Identity.Entities;
using HotelListing.Identity.Enums;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Identity.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;

        public AuthManager(UserManager<ApiUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IdentityError>> RegisterUser(UserDTO dto)
        {
            ApiUser entity = _mapper.Map<ApiUser>(dto);
            entity.UserName = dto.Email;

            IdentityResult result = await _userManager.CreateAsync(entity, dto.Password);
            if (result.Succeeded == false)
            {
                return result.Errors;
            }

            result = await _userManager.AddToRoleAsync(entity, RoleEnums.User.ToString());
            return result.Errors;
        }
    }
}
