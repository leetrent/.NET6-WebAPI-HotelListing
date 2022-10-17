using HotelListing.Identity.Constants;
using HotelListing.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenService(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateNewToken(ApiUser apiUser)
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

        public async Task<string> CreateRefreshToken(ApiUser apiUser)
        {
            string logSnippet = "[TokenService][CreateRefreshToken] =>";

            await _userManager.RemoveAuthenticationTokenAsync(apiUser, ProjectConstants.LoginProvider, ProjectConstants.TokenName);
            string newToken = await _userManager.GenerateUserTokenAsync(apiUser, ProjectConstants.TokenProvider, ProjectConstants.Purpose);
            Console.WriteLine($"{logSnippet} (newToken)........: '{newToken}'");

            IdentityResult result = await _userManager.SetAuthenticationTokenAsync(apiUser, ProjectConstants.LoginProvider, ProjectConstants.TokenName, newToken);
            Console.WriteLine($"{logSnippet} (result.Succeeded): '{result.Succeeded}'");


            return newToken;
        }

        public async Task<ApiUser?> FindUser(string token)
        {
            string logSnippet = "[TokenService][FindUser] =>";
            Console.WriteLine($"{logSnippet} (token): '{token}'");

            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(token);

            // EXTRACT ApiUser FROM TOKEN
            IEnumerable<Claim> claimsEnumerable = securityToken.Claims;
            List<Claim> claimsList = claimsEnumerable.ToList();
            Claim? claim = claimsList.FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email);
            string? userName = claim?.Value;
            Console.WriteLine($"{logSnippet} (userName): '{userName}'");

            ApiUser apiUser = await _userManager.FindByNameAsync(userName);
            Console.WriteLine($"{logSnippet} (apiUser == null): '{apiUser == null}'");
            return apiUser;
        }
    }
}
