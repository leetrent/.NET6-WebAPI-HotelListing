using AutoMapper;
using HotelListing.Data;
using HotelListing.Services.DTOs;

namespace HotelListing.Services.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Country, CountryCreateDTO>().ReverseMap();
            CreateMap<Country, CountryGetDTO>().ReverseMap();
        }
    }
}
