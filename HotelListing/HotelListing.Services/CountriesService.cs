using AutoMapper;
using HotelListing.Data;
using HotelListing.Data.Repositories.Interfaces;
using HotelListing.Services.DTOs;
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

        public async Task<CountryDTO> Create(CountryCreateDTO dto)
        {
            Country entityToCreate = _mapper.Map<Country>(dto);
            Country createdEntity = await _repository.Create(entityToCreate);
            return _mapper.Map<CountryDTO>(createdEntity);
        }

        public async Task<CountryDTO> RetrieveById(int id)
        {
            return _mapper.Map<CountryDTO>(await _repository.RetrieveById(id));
        }

        public async Task<List<CountryDTO>> RetrieveAll()
        {
            return _mapper.Map<List<CountryDTO>>(await _repository.RetrieveAll());
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