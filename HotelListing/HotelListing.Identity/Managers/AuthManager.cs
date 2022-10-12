using AutoMapper;
using HotelListing.Identity.DTOs;
using HotelListing.Identity.Entities;
using HotelListing.Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HotelListing.Identity.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
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

        public async Task<AuthResponseDTO?> Login(LoginDTO loginDTO)
        {
            ApiUser apiUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (apiUser != null)
            {
                bool userIsAuthenticated = await _userManager.CheckPasswordAsync(apiUser, loginDTO.Password);

                if (userIsAuthenticated)
                {
                    string securityToken = await this.generateToken(apiUser);
                    return new AuthResponseDTO
                    {
                        UserId = apiUser.Id,
                        Token = securityToken,
                    };
                }
            }
            return null;
        }

        private async Task<string> generateToken(ApiUser apiUser)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //IList<Claim> userClaims = await _userManager.GetClaimsAsync(apiUser);
            //IList<string> userRoles = await _userManager.GetRolesAsync(apiUser);
            //IEnumerable<Claim> roleClaims = userRoles.Select(rc => new Claim(ClaimTypes.Role, rc)).ToList();

            //IList<Claim> tokenClaims = new List<Claim>()
            //{
            //    new Claim("uid", apiUser.Id),
            //    new Claim(JwtRegisteredClaimNames.Sub, apiUser.Email),
            //    new Claim(JwtRegisteredClaimNames.Email, apiUser.Email),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};

            //tokenClaims.Union(userClaims);
            //tokenClaims.Union(roleClaims);

            var roles = await _userManager.GetRolesAsync(apiUser);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(apiUser);

            var tokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, apiUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, apiUser.Email),
                new Claim("uid", apiUser.Id),
            }.Union(userClaims).Union(roleClaims);


            string tokenIssuer = _configuration["JwtSettings:Issuer"];
            string tokenAudience = _configuration["JwtSettings:Audience"];
            string durationInMinutes = _configuration["JwtSettings:DurationInMinutes"];
            int minutes = Convert.ToInt32(durationInMinutes);
            DateTime tokenExpiration = DateTime.Now.AddMinutes(minutes);

            JwtSecurityToken securityToken = new JwtSecurityToken
            (
                issuer: tokenIssuer,
                audience: tokenAudience,
                claims: tokenClaims,
                expires: tokenExpiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
