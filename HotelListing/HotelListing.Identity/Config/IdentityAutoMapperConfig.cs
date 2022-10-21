using AutoMapper;
using HotelListing.Identity.DTOs;
using HotelListing.Identity.Entities;

namespace HotelListing.Identity.Config
{
    public class IdentityAutoMapperConfig : Profile
    {
        public IdentityAutoMapperConfig()
        {
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}