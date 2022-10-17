﻿using AutoMapper;
using HotelListing.Identity.Constants;
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

        public async Task<string?> Login(LoginDTO loginDTO)
        {
            ApiUser apiUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (apiUser != null)
            {
                bool userIsAuthenticated = await _userManager.CheckPasswordAsync(apiUser, loginDTO.Password);
                if (userIsAuthenticated)
                {
                    string securityToken = await this.generateToken(apiUser);
                    Console.WriteLine($"[AuthManager][Login] (securityToken): '{securityToken}'");
                    return securityToken;
        
                }
            }
            return null;
        }

        private async Task<string> generateToken(ApiUser apiUser)
        {
            string key = _configuration["JwtSettings:Key"];
            Console.WriteLine($"(key): '{key}'");

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IList<Claim> userClaims = await _userManager.GetClaimsAsync(apiUser);
            IList<string> userRoles = await _userManager.GetRolesAsync(apiUser);
            List<Claim> roleClaims = userRoles.Select(rc => new Claim(ClaimTypes.Role, rc)).ToList();
            
            IEnumerable<Claim> tokenClaims = new List<Claim>
            {               
                new Claim("uid", apiUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, apiUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, apiUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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

        private async Task<string> CreateRefreshToken(ApiUser apiUser)
        {
            await _userManager.RemoveAuthenticationTokenAsync(apiUser, ProjectConstants.LoginProvider, ProjectConstants.TokenName);
            string newToken = await _userManager.GenerateUserTokenAsync(apiUser, ProjectConstants.TokenProvider, ProjectConstants.Purpose);
            IdentityResult result = await _userManager.SetAuthenticationTokenAsync(apiUser, ProjectConstants.LoginProvider, ProjectConstants.TokenName, newToken);
             return newToken;
        }

        public async Task<string?> RefreshToken(string oldToken)
        {
            Console.WriteLine($"[AuthManager][RefreshToken] (newToken): '{oldToken}'");

            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(oldToken);

            // Extract ApiUser from old token
            IEnumerable<Claim> claimsEnumerable = securityToken.Claims;
            List<Claim> claimsList = claimsEnumerable.ToList();
            Claim? claim = claimsList.FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email);
            string? userName = claim?.Value;

            ApiUser apiUser = await _userManager.FindByNameAsync(userName);
            Console.WriteLine($"[AuthManager][RefreshToken] (apiUser == null): '{apiUser == null}'");
            if (apiUser == null)
            {
                return null;
            }

            await _userManager.UpdateSecurityStampAsync(apiUser);
            string newToken = await this.CreateRefreshToken(apiUser);
            Console.WriteLine($"[AuthManager][RefreshToken] (newToken): '{newToken}'");
            return newToken;

        }
    }
}
