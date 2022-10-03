using AutoMapper;
using HotelListing.Data;
using HotelListing.Data.Repositories.Interfaces;
using HotelListing.Services.DTOs;
using HotelListing.Services.DTOs.Country;
using HotelListing.Services.DTOs.Hotel;
using HotelListing.Services.Interfaces;

namespace HotelListing.Services
{
    public class HotelsService : IHotelsService
    {
        private readonly IHotelsRepository _repository;
        private readonly IMapper _mapper;

        public HotelsService(IHotelsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<HotelGetDTO>> RetrieveAll()
        {
            return _mapper.Map<List<HotelGetDTO>>(await _repository.RetrieveAll());
        }

        public async Task<HotelGetDTO> RetrieveById(int id)
        {
            return _mapper.Map<HotelGetDTO>(await _repository.RetrieveById(id));
        }

        public async Task<HotelGetDTO> Create(HotelCreateDTO dto)
        {
            Hotel entityToCreate = _mapper.Map<Hotel>(dto);
            int newHotelId = await _repository.Create(entityToCreate);

            return await this.RetrieveById(newHotelId);
        }

        public async Task<HotelGetDTO> Update(HotelUpdateDTO dto)
        {
            Hotel entityToUpdate = _mapper.Map<Hotel>(dto);
            int updatedEntityId = await _repository.Update(entityToUpdate);

            return await this.RetrieveById(updatedEntityId);
        }

        public async Task<int> Delete(int id)
        {
            return await _repository.Delete(id);
        }

    }
}