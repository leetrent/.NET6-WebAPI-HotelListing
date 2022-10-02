using AutoMapper;
using HotelListing.Data;
using HotelListing.Data.Repositories.Interfaces;
using HotelListing.Services.DTOs;
using HotelListing.Services.DTOs.Country;
using HotelListing.Services.Interfaces;

namespace HotelListing.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _repository;
        private readonly IMapper _mapper;

        public CountriesService(ICountriesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CountryGetDTO>> RetrieveAll()
        {
            return _mapper.Map<List<CountryGetDTO>>(await _repository.RetrieveAll());
        }

        public async Task<CountryGetDTO> RetrieveById(int id)
        {
            return _mapper.Map<CountryGetDTO>(await _repository.RetrieveById(id));
        }

        public async Task<CountryGetDTO> Create(CountryCreateDTO dto)
        {
            Country entityToCreate = _mapper.Map<Country>(dto);
            int newCountryId = await _repository.Create(entityToCreate);

            Console.WriteLine($"[CountriesService][Create] => (newCountryId): '{newCountryId}'");

            return await this.RetrieveById(newCountryId);
        }

        public async Task<CountryGetDTO> Update(CountryUpdateDTO dto)
        {
            Country entityToUpdate = _mapper.Map<Country>(dto);
            int updatedCountryId = await _repository.Update(entityToUpdate);

            Console.WriteLine($"[CountriesService][Update] => (updatedCountryId): '{updatedCountryId}'");

            return await this.RetrieveById(updatedCountryId);
        }

        public async Task<int> Delete(int id)
        {
            return await _repository.Delete(id);
        }

    }
}