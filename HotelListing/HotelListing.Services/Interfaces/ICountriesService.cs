using HotelListing.Data;
using HotelListing.Services.DTOs;

namespace HotelListing.Services.Interfaces
{
    public interface ICountriesService
    {
        Task<CountryGetDTO> Create(CountryCreateDTO dto);
        Task<Country> RetrieveById(int id);
        Task<List<Country>> RetrieveAll();
        Task<Country> Update(Country country);
        Task<int> Delete(int id);
    }
}