using AutoMapper;
using HotelListing.Identity.DTOs;
using HotelListing.Identity.Entities;
using HotelListing.Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HotelListing.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public readonly ITokenService _tokenService;

        public AuthService(UserManager<ApiUser> userManager, IConfiguration configuration, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
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
        public async Task<TokenDTO?> Login(LoginDTO loginDTO)
        {
            string logSnippet = "[AuthService][Login] =>";
            Console.WriteLine($"{logSnippet} (loginDTO.Email): '{loginDTO.Email}'");

            ApiUser apiUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            Console.WriteLine($"{logSnippet} (apiUser == null): '{apiUser == null}'");

            if (apiUser != null)
            {
                bool userIsAuthenticated = await _userManager.CheckPasswordAsync(apiUser, loginDTO.Password);
                if (userIsAuthenticated)
                {
                    string securityToken = await _tokenService.CreateNewToken(apiUser);
                    Console.WriteLine($"{logSnippet} (securityToken): '{securityToken}'");
                    return new TokenDTO { Token = securityToken };
                }
            }
            return null;
        }

        public async Task<TokenDTO?> RefreshToken(TokenDTO oldToken)
        {
            string logSnippet = "[AuthService][RefreshToken] =>";
            Console.WriteLine($"[AuthManager][RefreshToken] (oldToken.Token): '{oldToken.Token}'");

            ApiUser? apiUser = await _tokenService.FindUser(oldToken.Token);
            Console.WriteLine($"{logSnippet} (apiUser == null): '{apiUser == null}'");

            if (apiUser == null)
            {
                return null;
            }

            string? newToken = await _tokenService.CreateRefreshToken(apiUser);
            Console.WriteLine($"[AuthManager][RefreshToken] (newToken): '{newToken}'");

            if (newToken != null)
            {
                return new TokenDTO { Token = newToken };
            }

            return null;
        }
    }
}
