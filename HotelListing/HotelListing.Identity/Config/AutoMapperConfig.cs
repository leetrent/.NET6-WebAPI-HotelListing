using AutoMapper;
using HotelListing.Identity.DTOs;
using HotelListing.Identity.Entities;

namespace HotelListing.Identity.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}