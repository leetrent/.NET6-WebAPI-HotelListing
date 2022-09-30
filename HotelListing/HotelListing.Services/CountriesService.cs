using HotelListing.Data;
using HotelListing.Data.Repositories;
using HotelListing.Data.Repositories.Interfaces;
using HotelListing.Services.Interfaces;

namespace HotelListing.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _repository;

        //public CountriesService()
        //{
        //    _repository = new CountriesRepository();
        //}
        public CountriesService(ICountriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Country> Create(Country country)
        {
            return await _repository.Create(country);
        }

        public async Task<Country> RetrieveById(int id)
        {
            return await _repository.RetrieveById(id);
        }

        public async Task<List<Country>> RetrieveAll()
        {
            return await _repository.RetrieveAll();
        }

        public async Task<Country> Update(Country country)
        {
            Country updatedCountry = await _repository.Update(country);
            Console.WriteLine($"[CountriesService][Update] => (updatedCountry): '{updatedCountry.Id}' / '{updatedCountry.ShortName}' / '{updatedCountry.Name}'");
            return updatedCountry;
        }

        public async Task<int> Delete(int id)
        {
            return await _repository.Delete(id);
        }

    }
}