using HotelListing.Services.DTOs.Hotel;

namespace HotelListing.Services.Interfaces
{
    public interface IHotelsService
    {
        Task<List<HotelGetDTO>> RetrieveAll();
        Task<HotelGetDTO> RetrieveById(int id);
        Task<HotelGetDTO> Create(HotelCreateDTO dto);
        Task<HotelGetDTO> Update(HotelUpdateDTO dto);
        Task<int> Delete(int id);
        Task<bool> HotelExists(int id);
        Task<bool> CountryExists(int id);
    }
}
