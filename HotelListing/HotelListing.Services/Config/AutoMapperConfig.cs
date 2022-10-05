using AutoMapper;
using HotelListing.Data;
using HotelListing.Services.DTOs.Country;
using HotelListing.Services.DTOs.Hotel;

namespace HotelListing.Services.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CountryBaseDTO>().ReverseMap();
            CreateMap<Country, CountryCreateDTO>().ReverseMap();
            CreateMap<Country, CountryGetDTO>().ReverseMap();
            CreateMap<Country, CountryUpdateDTO>().ReverseMap();

            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel, HotelBaseDTO>().ReverseMap();
            CreateMap<Hotel, HotelCreateDTO>().ReverseMap();
            CreateMap<Hotel, HotelGetDTO>().ReverseMap();
            CreateMap<Hotel, HotelUpdateDTO>().ReverseMap();
        }
    }
}