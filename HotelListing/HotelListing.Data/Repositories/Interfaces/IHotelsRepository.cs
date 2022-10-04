namespace HotelListing.Data.Repositories.Interfaces
{
    public interface IHotelsRepository
    {
        Task<List<Hotel>> RetrieveAll();
        Task<Hotel> RetrieveById(int id);
        Task<int> Create(Hotel hotel);
        Task<int> Update(Hotel hotel);
        Task<int> Delete(int id);
        Task<bool> HotelExists(int id);
        Task<bool> CountryExists(int id);
    }
}