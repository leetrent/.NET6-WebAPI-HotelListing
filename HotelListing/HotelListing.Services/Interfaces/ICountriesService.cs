using HotelListing.Data;
using HotelListing.Services.DTOs;

namespace HotelListing.Services.Interfaces
{
    public interface ICountriesService
    {
        Task<CountryDTO> Create(CountryCreateDTO dto);
        Task<CountryDTO> RetrieveById(int id);
        Task<List<CountryDTO>> RetrieveAll();
        Task<Country> Update(Country country);
        Task<int> Delete(int id);
    }
}