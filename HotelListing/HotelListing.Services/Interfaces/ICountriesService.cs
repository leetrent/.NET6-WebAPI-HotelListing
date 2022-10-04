using HotelListing.Data;
using HotelListing.Services.DTOs;
using HotelListing.Services.DTOs.Country;

namespace HotelListing.Services.Interfaces
{
    public interface ICountriesService
    {
        Task<List<CountryGetDTO>> RetrieveAll();
        Task<CountryGetDTO> RetrieveById(int id);
        Task<CountryGetDTO> Create(CountryCreateDTO dto);
        Task<CountryGetDTO> Update(CountryUpdateDTO dto);
        Task<int> Delete(int id);
        Task<bool> CountryExists(int id);
    }
}