using HotelListing.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Identity.Services
{
    public interface ITokenService
    {
        Task<string> CreateNewToken(ApiUser apiUser);
        Task<string?> CreateRefreshToken(ApiUser apiUser);
        Task<ApiUser?> FindUser(string token);
    }
}
